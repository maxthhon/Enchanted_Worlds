using Godot;
using System;

public partial class SavesMenu : BaseMenu
{
	private Sprite2D book;
	private Tween tween;

	public override void _Ready()
	{
		base._Ready();

		book = GetNode<Sprite2D>("Book");
		tween = GetTree().CreateTween();

		LaunchBookToCenter();
	}

	private void LaunchBookToCenter()
	{
		Vector2 screenCenter = GetViewport().GetVisibleRect().Size / 2;

		if (tween != null && tween.IsRunning())
			tween.Kill();

		tween = CreateTween();

		tween.Parallel().TweenProperty(book, "global_position", screenCenter, 1.0f)
			.SetTrans(Tween.TransitionType.Quint)
			.SetEase(Tween.EaseType.Out);
	}
	
	public override void HandleTransitionComplete(bool isSwitchedOn)
	{
		if (!isSwitchedOn)
			return;

		switch (buttonPressed)
		{
			case 1:
				GetTree().ChangeSceneToFile("res://scenes/main/MainMenu.tscn");
				break;
			case 2:
				GetTree().ChangeSceneToFile("res://scenes/main/SettingsMenu.tscn");
				break;
			case 3:
				
				break;
		}
	}
}
