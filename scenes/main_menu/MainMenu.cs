using Godot;
using System;

public partial class MainMenu : Node2D
{
	private AnimatedSprite2D animationPlayer;
	private AudioStreamPlayer audioPlayer;
	
	public override void _Ready()
	{
		animationPlayer = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		animationPlayer.Play("stage");
	}

	private void _on_button_2_pressed()
	{
		GetTree().Quit();
	}
	
	private void _on_button_pressed()
	{
		animationPlayer.Play("go");
		GetNode<Timer>("play_timer").Start();
	}

	private void _on_button_3_pressed()
	{
		GetTree().ChangeSceneToFile("res://scenes/menu_about.tscn");
	}

	private void _on_timer_timeout()
	{
		animationPlayer.Play("stage");
		GetTree().ChangeSceneToFile("res://scenes/level_manager.tscn");
	}

	private void _on_audio_stream_player_finished()
	{
		audioPlayer.Play();
	}
}
