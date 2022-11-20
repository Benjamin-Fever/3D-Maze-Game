using Godot;
using System;

public class Menu : Control
{
    // Declare member variables here. Examples:
    // private int a = 2;
    // private string b = "text";

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        GetNode<Control>("idField").Hide();
    }

    public void hostClicked(){
        GetTree().ChangeScene("res://Scene/Basic Scene.tscn");
    }

    public void connectClicked(){
        GetNode<Control>("idField").Show();
    }

    public void confirmClicked(){
        GameManager.seed = Int32.Parse(GetNode<Control>("idField").GetNode<TextEdit>("idEditor").Text);
        GetTree().ChangeScene("res://Scene/Map.tscn");
    }

//  // Called every frame. 'delta' is the elapsed time since the previous frame.
//  public override void _Process(float delta)
//  {
//      
//  }
}
