using Godot;
using System;

public class Harpy : KinematicBody2D
{
    private KinematicBody2D player;
    Vector2 UP = new Vector2(0, -1);
    Vector2 motion = new Vector2();
    const int MAXSPEED = 50;
    const int ACCEL = 10;

    AnimatedSprite harpySprite;

    AudioStreamPlayer2D wings;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        harpySprite = GetNode<AnimatedSprite>("AnimatedSprite");
        player = null;
        motion = Vector2.Zero;
        wings = GetNode<AudioStreamPlayer2D>("Wings");
        wings.Play();
    }

    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (motion.x <= 0)
        {
            harpySprite.FlipH = true;
        }
        else
        {
            harpySprite.FlipH = false;
        }

        if (player != null)
        {
            motion = Position.DirectionTo(player.Position);
            harpySprite.Play("Fly");

            //GD.Print(motion);
        }
        else
        {
            harpySprite.Play("Idle");
            motion = Vector2.Zero;
        }

        motion = motion.Normalized() * MAXSPEED;
        motion = MoveAndSlide(motion, UP);
    }

    private void _on_Area2D_body_entered(KinematicBody2D body)
    {
        if (body.Name == "Player")
        {
            player = body;
            //D.Print("Player is in the zone");
        }
    }

    private void _on_Area2D_body_exited(KinematicBody2D body)
    {
        player = null;
        //GD.Print("Player left the zone");
    }
}
