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
    //  public override void _Process(float delta)
    //  {
    //      
    //  }

    public void _on_Area2D_body_entered(KinematicBody2D body)
    {
        if (body.Name == "Player")
        {
            GetTree().ChangeScene("res://World2.tscn");
        }
    }
}
