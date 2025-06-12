using Godot;
using System;

public partial class MapExit : StaticBody2D
{
	[Export]
	public int LevelId;
	
	public string PreviousLevelId;
	
	public string LevelPath;
	
	[Export]
	public bool Enabled = false;

	private Character playerInArea = null;
	private AnimatedSprite2D sprite;
	private CollisionShape2D collider;

	public override void _Ready()
	{
		LevelPath = GameManager.Instance.Profile.GetScenePathById(LevelId);
		if (LevelId > 1)
		{
			PreviousLevelId = (LevelId - 1).ToString();
		} else {
			PreviousLevelId = "";
		}
		sprite = GetNodeOrNull<AnimatedSprite2D>("AnimatedSprite2D");
		collider = GetNodeOrNull<CollisionShape2D>("CollisionShape2D");

		if (Enabled != true)
		{
			bool completed = GameManager.Instance?.Profile?.HasCompletedLevel(PreviousLevelId) ?? false;	

			if (sprite != null)
				sprite.Frame = completed ? 0 : 1;

			if (collider != null)
				collider.Disabled = completed;
				
		} else {
			sprite.Frame = 0;
			collider.Disabled = true;
		}
	}

	public override void _Process(double delta)
	{
		if (playerInArea != null && playerInArea.TryInterrupt())
		{
			if (GameManager.Instance?.Profile != null)
			{
				GD.Print("Персонаж взаимодействует с выходом!");
				if (LevelPath != null)
				{
				var level = GetTree().CurrentScene as BaseLevel;
				if (level != null)
					level.TransitionToLevel(LevelPath);
				else
					GD.PrintErr("Не удалось найти уровень для проверки выхода!");
			}
			else
			{
				var blackout = GetParent().GetNode<DirectionalLight2D>("Blackout");
				if (blackout != null && blackout.HasMethod("Blink"))
					blackout.Call("Blink");
				else
					GD.PrintErr("Не найден blackout или у него нет метода Blink!");
			}
		}
		else
		{
			GD.PrintErr("GameManager.Instance или Profile == null");
		}
		playerInArea = null; // Чтобы не срабатывало повторно
		}
	}

	public void _on_area_2d_body_entered(Node body)
	{
		if (body is Character character)
		{
			playerInArea = character;
			GD.Print("Игрок вошёл в зону выхода.");
		}
	}

	public void _on_area_2d_body_exited(Node body)
	{
		if (body is Character character && playerInArea == character)
		{
			playerInArea = null;
			GD.Print("Игрок вышел из зоны выхода.");
		}
	}
}
