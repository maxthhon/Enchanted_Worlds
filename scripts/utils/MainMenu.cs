using Godot;
using System;

public partial class MainMenu : BaseMenu
{
	private Sprite2D book;
	private Label label;
	private Tween tween;

	public override void _Ready()
	{
		base._Ready();

		book = GetNode<Sprite2D>("Book");
		label = GetNode<Label>("GameName");
		tween = GetTree().CreateTween();

		LaunchBookToCenter();
	}

	private void LaunchBookToCenter()
	{
		Vector2 screenCenter = GetViewport().GetVisibleRect().Size / 2;
		screenCenter.X -= GD.RandRange(75, 80);
		float targetRotation = (float)Mathf.DegToRad(GD.RandRange(-6f, -3f));

		if (tween != null && tween.IsRunning())
			tween.Kill();

		tween = CreateTween();

		tween.Parallel().TweenProperty(book, "global_position", screenCenter, 1.0f)
			.SetTrans(Tween.TransitionType.Quint)
			.SetEase(Tween.EaseType.Out);

		tween.Parallel().TweenProperty(book, "rotation", targetRotation, 1.0f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.Out);

		tween.TweenProperty(label, "global_position", new Vector2(10, (screenCenter.Y / 10) - 5), 2.0f)
			.SetTrans(Tween.TransitionType.Elastic)
			.SetEase(Tween.EaseType.InOut);
	}
	
	public override void HandleTransitionComplete(bool isSwitchedOn)
	{
		if (!isSwitchedOn)
			return;

		switch (buttonPressed)
		{
			case 1:
				GetTree().ChangeSceneToFile("res://scenes/world/TestLevel.tscn");
				break;
			case 2:
				GetTree().ChangeSceneToFile("res://scenes/main/SettingsMenu.tscn");
				break;
			case 3:
				GetTree().Quit();
				break;
		}
	}

	private void _on_button_pressed()
	{
		SetButtonPressed(1);
	}

	private void _on_button_2_pressed()
	{
		SetButtonPressed(2);
	}

	private void _on_quit_button_pressed()
	{
		SetButtonPressed(3);
	}
}
