using Godot;
using System;

public partial class MapExit : StaticBody2D
{
	[Export]
	public string LevelId { get; set; }
	
	[Export]
	public string NextLevelPath { get; set; }
	
	public void _on_area_2d_body_entered(Node body)
	{
		if (body is Character character)
		{
			// Убедись, что GameManager.Instance и Profile НЕ null
			if (GameManager.Instance?.Profile != null)
			{
				if (character.isInterupted)
				{
					// Действие, если персонаж взаимодействует (isInterupted == true)
					GD.Print("Персонаж взаимодействует с выходом!");
					
					var level = GetTree().CurrentScene as BaseLevel;
					if (level != null)
						level.TransitionToLevel(NextLevelPath);
					else
						GD.PrintErr("Не удалось найти уровень для проверки выхода!");
		
				}
				else
				{
					// Действие, если персонаж НЕ взаимодействует (isInterupted == false)
					GD.Print("Для выхода нужно взаимодействие (нажми пробел)!");
				}
			}
			else
			{
				GD.PrintErr("GameManager.Instance или Profile == null");
			}
		}
	}
}
