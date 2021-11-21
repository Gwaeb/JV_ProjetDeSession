using Godot;
using System;

public class Player : KinematicBody2D
{
    Vector2 UP = new Vector2(0, -1);
    const int GRAVITY = 15;
    const int MAXFALLSPEED = 200;
    const int MAXSPEED = 75;
    const int JUMPFORCE = 300;
    const int ACCEL = 10;

    enum State
    {
        MOVE,
        ATTACK
    }
    State state = State.MOVE;
    Vector2 motion = new Vector2();

    //Sprite
    AnimatedSprite playerSprite;
    AnimationTree animationTree;
    AnimationNodeStateMachinePlayback animationState;
    bool facing_right = true;


    CollisionShape2D swordCollisonR;
    CollisionShape2D swordCollisonL;

    KinematicBody2D targetEnnemy;



    AudioStreamPlayer2D sword;
    AudioStreamPlayer2D swordInFlesh;


    Label label;
    int score;

    Label labelDebug;
    bool debug;

    public override void _Ready()
    {
        playerSprite = GetNode<AnimatedSprite>("AnimatedSpritePlayer");
        animationTree = GetNode<AnimationTree>("AnimationTree");
        animationTree.Active = true;
        animationState = (AnimationNodeStateMachinePlayback)animationTree.Get("parameters/playback");

        swordCollisonR = GetNode<CollisionShape2D>("Area2DR/CollisionShape2DRight");
        swordCollisonL = GetNode<CollisionShape2D>("Area2DL/CollisionShape2DLeft");
        swordCollisonR.Disabled = true;
        swordCollisonL.Disabled = true;

        label = GetNode<Label>("Label");
        score = 0;
        labelDebug = GetNode<Label>("LabelDebug");
        debug = false;



        sword = GetNode<AudioStreamPlayer2D>("Sword");
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_debug"))
        {
            debug = !debug;
        }
        if (!debug)
        {
            labelDebug.Text = "";
        }
        label.Text = "Score: " + score;
        move_state(delta);
        switch (state)
        {
            case State.MOVE:
                move_state(delta);
                break;
            case State.ATTACK:
                attack_state(delta);
                break;
        }
    }

    private void move_state(float delta)
    {
        Gravity(delta);

        if (Input.IsActionPressed("ui_left"))
        {
            playerSprite.FlipH = true;
            animationState.Travel("Run");
            motion.x -= ACCEL;
            if (motion.x <= -MAXSPEED)
            {
                motion.x = -MAXSPEED;
            }

            if (debug)
            {
                labelDebug.Text = "State: Move Left";
            }

        }
        else if (Input.IsActionPressed("ui_right"))
        {
            playerSprite.FlipH = false;
            animationState.Travel("Run");
            motion.x += ACCEL;
            if (motion.x >= MAXSPEED)
            {
                motion.x = MAXSPEED;
            }
            if (debug)
            {
                labelDebug.Text = "State: Move Right";
            }
        }
        else
        {
            motion.x = 0;
            animationState.Travel("Idle");


            if (debug)
            {
                labelDebug.Text = "State: Idle";
            }
        }

        if (IsOnFloor())
        {
            // On ne regarde qu'un seul fois et non le maintient de la touche
            if (Input.IsActionJustPressed("ui_up"))
            {
                motion.y = -JUMPFORCE;
                GD.Print($"motion.y = {motion.y}");
            }
        }

        if (!IsOnFloor())
        {
            if (motion.y < 0)
            {
                animationState.Travel("Jump");

                if (debug)
                {
                    labelDebug.Text = "State: Jump";
                }
            }
            else if (motion.y > 0)
            {
                animationState.Travel("Fall");

                if (debug)
                {
                    labelDebug.Text = "State: Fall";
                }
            }
        }

        motion = MoveAndSlide(motion, UP);

        if (Input.IsActionPressed("ui_attack"))
        {
            state = State.ATTACK;
        }
    }
    private void attack_state(float delta)
    {

        if (debug)
        {
            labelDebug.Text = "State: Attack";
        }
        if (!sword.Playing)
        {
            sword.Play();
        }
        animationState.Travel("Attack");
        if (targetEnnemy != null)
        {
            score += 10;
            targetEnnemy.QueueFree();
        }
        Gravity(delta);
        motion = MoveAndSlide(motion, UP);
    }

    public void attack_animation_finished()
    {
        sword.Stop();
        state = State.MOVE;
        swordCollisonL.Disabled = true;
        swordCollisonR.Disabled = true;
    }

    private void Gravity(float delta)
    {
        motion.y += GRAVITY;

        if (motion.y > MAXFALLSPEED)
        {
            motion.y = MAXFALLSPEED;
        }
    }


    public void _on_Area2D_body_entered(KinematicBody2D body)
    {

        GD.Print(body.IsInGroup("Harpy"));


        if (body.Name == "Harpy" || body.Name == "Harpy2" || body.Name == "Harpy3" || body.Name == "Harpy4")
        {
            targetEnnemy = body;
            GD.Print("you can die");
        }
    }

    private void _on_Area2D_body_exited(KinematicBody2D body)
    {
        if (targetEnnemy != null)
        {
            targetEnnemy = null;
            GD.Print("Ennemi left the zone");
        }
    }

    private void attack_hitbox()
    {
        if (playerSprite.FlipH == true)
        {
            swordCollisonR.Disabled = true;
            swordCollisonL.Disabled = false;
        }
        else
        {
            swordCollisonR.Disabled = false;
            swordCollisonL.Disabled = true;
        }
    }
}
