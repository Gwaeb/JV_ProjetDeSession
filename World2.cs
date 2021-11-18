using Godot;
using System;

public class World2 : Node2D
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {
        if (Input.IsActionJustPressed("ui_pause"))
        {
            GetTree().Paused = true;
            var options = GD.Load<PackedScene>("res://PauseMenu.tscn").Instance();
            GetTree().CurrentScene.AddChild(options);
        }

        if (Input.IsActionJustPressed("ui_lvl1"))
        {
            GetTree().ChangeScene("res://World1.tscn");
        }
        if (Input.IsActionJustPressed("ui_lvl2"))
        {
            GetTree().ChangeScene("res://World2.tscn");
        }
        if (Input.IsActionJustPressed("ui_endScene"))
        {
            GetTree().ChangeScene("res://EndMenu.tscn");
        }
    }

    public void _on_Area2D_body_entered(KinematicBody2D body)
    {
        if (body.Name == "Player")
        {
            GetTree().ChangeScene("res://World2.tscn");
        }
    }

    public void _on_EndGame_body_entered(KinematicBody2D body)
    {
        if (body.Name == "Player")
        {
            GetTree().ChangeScene("res://EndMenu.tscn");
        }
    }
}
