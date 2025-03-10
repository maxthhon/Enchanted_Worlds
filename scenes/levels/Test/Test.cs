using Godot;
using System;

public partial class Test : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void _on_music_finished()
	{
		GetNode<AudioStreamPlayer2D>("Music").Play();
	}
	
	public void _on_timer_timeout()
	{
		var blackout = GetNode<DirectionalLight2D>("Blackout");
		
		if (blackout.Energy >= 0) 
		{
			blackout.Energy -= 0.01f;
		}
	}
}
