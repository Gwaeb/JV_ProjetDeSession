using Godot;
using System;

public class PauseMenu : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {

    }

    public void _on_ButtonResume_pressed()
    {
        QueueFree();
        GetTree().Paused = false;
    }

    public void _on_ButtonOptions_pressed()
    {
        var options = GD.Load<PackedScene>("res://OptionsMenu.tscn").Instance();
        GetTree().CurrentScene.AddChild(options);
    }

    public void _on_ButtonExit_pressed()
    {
        GetTree().Paused = false;
        GetTree().ChangeScene("res://MainMenu.tscn");
    }
    //  // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(float delta)
    {

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
}
