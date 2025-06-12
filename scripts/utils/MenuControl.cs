using Godot;
using System;

public partial class MenuControl : CanvasLayer
{
	private bool _isPaused = false;
	protected Blackout blackout;
	public override void _Ready()
	{
		this.Visible = false;
		GD.Print("MenuControl: _Ready вызван, меню скрыто, обработка ввода включена.");
	
		blackout = GetParent().GetNode<Blackout>("Blackout");
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("ui_cancel"))
		{
			GD.Print("MenuControl: Нажата клавиша ui_cancel (Esc).");

			var baseLevel = GetTree().CurrentScene as BaseLevel;
			if (baseLevel == null)
			{
				GD.Print("MenuControl: BaseLevel не найден.");
				return;
			}

			if (_isPaused)
			{
				GD.Print("MenuControl: Снимаем паузу.");
				baseLevel.ResumeGame();
				_isPaused = false;
				this.Visible = false;
				GD.Print("MenuControl: Меню скрыто, игра продолжается.");
			}
			else
			{
				GD.Print("MenuControl: Ставим игру на паузу.");
				baseLevel.PauseGame();
				_isPaused = true;
				this.Visible = true;
				GD.Print("MenuControl: Меню показано, игра на паузе.");
			}
		}
	}

	private void _OnContinuePressed()
	{
		var baseLevel = GetTree().CurrentScene as BaseLevel;
		if (baseLevel != null)
		{
			baseLevel.ResumeGame();
			_isPaused = false;
			this.Visible = false;
			GD.Print("MenuControl: Продолжить игру.");
		}
	}

	private void _OnMapPressed()
	{
		if (blackout != null)
		{
			blackout.OnTransitionComplete += (isOn) =>
			{
				if (isOn)
					GetTree().ChangeSceneToFile("res://scenes/world/LevelMap.tscn");
			};

			blackout.SetBlackout(true);
		}
		else
		{
			GetTree().ChangeSceneToFile("res://scenes/world/LevelMap.tscn");
		}
	}

	private void _OnMenuPressed()
	{
		if (blackout != null)
		{
			blackout.OnTransitionComplete += (isOn) =>
			{
				if (isOn)
					GetTree().ChangeSceneToFile("res://scenes/main/MainMenu.tscn");
			};

			blackout.SetBlackout(true);
		}
		else
		{
			GetTree().ChangeSceneToFile("res://scenes/main/MainMenu.tscn");
		}
	}

	private void _OnExitPressed()
	{
		GetTree().Quit();
	}
	
	private void _on_button_5_pressed()
	{
		var currentScene = GetTree().CurrentScene;
		if (currentScene != null)
		{
			GD.Print("MenuControl: Перезапуск уровня...");
			GetTree().ReloadCurrentScene();
		}
		else
		{
			GD.PrintErr("MenuControl: Не удалось получить текущую сцену для перезапуска.");
		}
	}
}
