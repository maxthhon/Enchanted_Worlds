using Godot;
using System;

public partial class BaseMenu : Control
{
	protected Blackout blackout;
	protected AudioStreamPlayer audioPlayer;
	protected int buttonPressed;

	private Sprite2D book;
	private Tween tween;

	public override void _Ready()
	{
		audioPlayer = GetNode<AudioStreamPlayer>("AudioStreamPlayer");
		blackout = GetNode<Blackout>("Blackout");
		book = GetNode<Sprite2D>("Book");

		tween = GetTree().CreateTween();

		blackout.OnTransitionComplete += HandleTransitionComplete;
		blackout.SetBlackout(false);
		
		LaunchBookToCenter();
	}

	public void LaunchBookToCenter()
	{
		Vector2 screenCenter = GetViewport().GetVisibleRect().Size / 2;
		float targetRotation = (float)Mathf.DegToRad(GD.RandRange(-3f, 3f));

		if (tween != null && tween.IsRunning())
			tween.Kill();

		tween = CreateTween();

		// Добавляем сразу обе анимации — параллельно
		tween.Parallel().TweenProperty(book, "global_position", screenCenter, 1.0f)
			.SetTrans(Tween.TransitionType.Quint)
			.SetEase(Tween.EaseType.Out);

		tween.Parallel().TweenProperty(book, "rotation", targetRotation, 1.0f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.Out);
	}

	public void _on_button_pressed()
	{
		buttonPressed = 1;
		blackout.SetBlackout(true);
	}

	public void _on_button_2_pressed()
	{
		buttonPressed = 2;
		blackout.SetBlackout(true);
	}

	public void _on_quit_button_pressed()
	{
		buttonPressed = 3;
		blackout.SetBlackout(true);
	}

	public virtual void HandleTransitionComplete(bool isSwitchedOn)
	{
		if (!isSwitchedOn)
			return;

		switch (buttonPressed)
		{
			case 1:
				GetTree().ChangeSceneToFile("res://scenes/levels/Test2/test2.tscn");
				break;
			case 2:
				GetTree().ChangeSceneToFile("res://scenes/main_menu/Settings.tscn");
				break;
			case 3:
				GetTree().Quit();
				break;
		}
	}

	public virtual void _on_audio_stream_player_finished()
	{
		audioPlayer.Play();
	}
}
