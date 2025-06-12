using Godot;
using System;

public partial class Exit : StaticBody2D
{
	[Export]
	public string CurrentLevelId;
	
	public void _on_area_2d_body_entered(Node body)
	{
		if (body is Character)
		{
			// Получаем уровень и вызываем TryExitLevel
			var level = GetTree().CurrentScene as BaseLevel;
			if (level != null)
				level.TryToLevel(CurrentLevelId);
			else
				GD.PrintErr("Не удалось найти уровень для проверки выхода!");
		}
	}
}
