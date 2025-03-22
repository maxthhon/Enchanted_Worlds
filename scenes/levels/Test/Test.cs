using Godot;
using System;

public partial class Test : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		var blackout = (Blackout)GetNode<DirectionalLight2D>("Blackout");
		blackout.SetBlackout(false);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void _on_music_finished()
	{
		GetNode<AudioStreamPlayer2D>("Music").Play();
	}
}
