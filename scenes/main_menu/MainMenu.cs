using Godot;
using System;

public partial class MainMenu : Node2D
{
	private Blackout blackout;
	private AnimatedSprite2D animationPlayer;
	private AudioStreamPlayer audioPlayer;
	private int buttonPressed; 
	
	public override void _Ready()
	{
		animationPlayer = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		blackout = (Blackout)GetNode<DirectionalLight2D>("Blackout");
		blackout.OnTransitionComplete += HandleTransitionComplete;
		blackout.SetBlackout(false);
	}

	private void _on_button_2_pressed()
	{
		buttonPressed = 2;
		blackout.SetBlackout(true);
		//GetTree().Quit();
	}
	
	private void _on_button_pressed()
	{
		buttonPressed = 1;
		animationPlayer.Play("walk_down");
		blackout.SetBlackout(true);
	}

	private void _on_button_3_pressed()
	{
		buttonPressed = 3;
		blackout.SetBlackout(true);
		//GetTree().ChangeSceneToFile("res://scenes/main_menu/Settings.tscn");
	}
	
	private void HandleTransitionComplete(bool isSwitchedOn)
	{
		switch (buttonPressed)
		{
			case 1:
				GetTree().ChangeSceneToFile("res://scenes/levels/Test2/test2.tscn");
				break;
			case 2:
				GetTree().Quit();
				break;
			case 3:
				GetTree().ChangeSceneToFile("res://scenes/main_menu/Settings.tscn");
				break;
		}
	}

	private void _on_audio_stream_player_finished()
	{
		audioPlayer.Play();
	}
}
