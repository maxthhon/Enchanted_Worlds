using Godot;
using System;

public partial class LevelMap : BaseLevel
{
	public override void _Ready()
	{
		base._Ready();
		RestorePlayerSpawnerPosition();
	}

	private void RestorePlayerSpawnerPosition()
	{
		try
		{
			var profile = GameManager.Instance.Profile;
			var spawner = GetNodeOrNull<Node2D>("PlayerSpawner");
			if (profile.SavedPlayerPosition != null && spawner != null)
			{
				spawner.Position = profile.SavedPlayerPosition.Value;
				GD.Print($"Восстановлена позиция спавнера игрока: {spawner.Position}");

				// Также перемещаем игрока, если он уже создан
				var player = spawner.GetNodeOrNull<CharacterBody2D>("Character");
				if (player != null)
				{
					player.Position = profile.SavedPlayerPosition.Value;
					GD.Print($"Восстановлена позиция игрока: {player.Position}");
				}
			}
		}
		catch (Exception ex)
		{
			GD.PrintErr($"Ошибка восстановления позиции спавнера или игрока: {ex.Message}");
		}
	}

	public override void _Process(double delta)
	{
		base._Process(delta);
	}
}
