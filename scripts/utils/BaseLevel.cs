using Godot;
using System;

public partial class BaseLevel : Node2D
{
	protected Blackout blackout;
	public int RequiredCoins { get; set; } = 0; // По умолчанию не требуется монет
	public string NextLevelPath { get; set; } // По умолчанию не требуется монет

	public override void _Ready()
	{
		GetTree().Paused = false;
		blackout = GetNodeOrNull<Blackout>("Blackout");

		if (blackout != null)
		{
			blackout.SetBlackout(false);
		}

		GameManager.Instance.Profile.ClearLocalInventory();
		
	}

	public virtual void PauseGame()
	{
		GetTree().Paused = true;
	}

	public virtual void ResumeGame()
	{
		GetTree().Paused = false;
	}

	public virtual void TransitionToLevel(string path)
	{
		if (blackout != null)
		{
			blackout.OnTransitionComplete += (isOn) =>
			{
				if (isOn)
					GetTree().ChangeSceneToFile(path);
			};

			blackout.SetBlackout(true);
		}
		else
		{
			GetTree().ChangeSceneToFile(path);
		}
	}

	/// <summary>
	/// Проверяет, достаточно ли монет у игрока для выхода, и завершает уровень.
	/// </summary>
	public virtual void TryExitLevel()
	{
		var profile = GameManager.Instance.Profile;
		
		if (profile != null && profile.HasItem("Coin", RequiredCoins))
		{
			GD.Print("Достаточно монет для выхода!");
			GameManager.Instance.ExitLevel();
		}
		else
		{
			GD.Print("Недостаточно монет для выхода!");
			// Выводим весь инвентарь игрока для отладки/информирования
			if (profile != null)
			{
				var items = profile.GetAllItems();
				foreach (var item in items)
				{
					GD.Print($"Инвентарь: {item.Key} x{item.Value}");
				}
			}
			// Мигнуть затемнением экрана для уведомления игрока
			if (blackout != null)
			{
				blackout.Blink();
			}
			// Можно добавить дополнительное уведомление игроку
		}
	}
	
	public virtual void TryToLevel(string CurrentLevelId)
	{
		var profile = GameManager.Instance.Profile;
		profile.MarkLevelCompleted(CurrentLevelId);
		if (profile != null && profile.HasItem("Coin", RequiredCoins))
		{
			GD.Print("Достаточно монет для выхода!");
			GameManager.Instance.OnLevelCompleted();
			TransitionToLevel(NextLevelPath);
		}
		else
		{
			GD.Print("Недостаточно монет для выхода!");
			// Выводим весь инвентарь игрока для отладки/информирования
			if (profile != null)
			{
				var items = profile.GetAllItems();
				foreach (var item in items)
				{
					GD.Print($"Инвентарь: {item.Key} x{item.Value}");
				}
			}
			// Мигнуть затемнением экрана для уведомления игрока
			if (blackout != null)
			{
				blackout.Blink();
			}
			// Можно добавить дополнительное уведомление игроку
		}
	}
}
