using Godot;
using System;

public partial class BaseMenu : Control
{
	protected Blackout blackout;
	protected AudioStreamPlayer audioPlayer;
	protected int buttonPressed;

	public override void _Ready()
	{
		GetTree().Paused = false;
		audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		blackout = GetNode<Blackout>("Blackout");

		blackout.OnTransitionComplete += HandleTransitionComplete;
		blackout.SetBlackout(false);
	}

	public void SetButtonPressed(int value)
	{
		buttonPressed = value;
		blackout.SetBlackout(true);
	}

	public virtual void HandleTransitionComplete(bool isSwitchedOn)
	{
		// Переопределяется в MainMenu
	}

}
