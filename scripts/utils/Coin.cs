using Godot;
using System;

public partial class Coin : StaticBody2D
{
	private AnimatedSprite2D _animationPlayer;
	private BaseLevel _baseLevel;
	private CoinCounter _coinCounter;

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_animationPlayer.Play();

		// Инициализация ссылок на BaseLevel и CoinCounter
		_baseLevel = GetTree().CurrentScene as BaseLevel;
		_coinCounter = GetTree().CurrentScene.GetNodeOrNull<CoinCounter>("CoinCounter");

		int requiredCoins = _baseLevel != null ? _baseLevel.RequiredCoins : 0;
		GD.Print($"[Ready] BaseLevel найден: {_baseLevel != null}");
		GD.Print($"[Ready] CoinCounter найден: {_coinCounter != null}");
		GD.Print($"[Ready] RequiredCoins на уровне: {requiredCoins}");

		// Вызываем UpdateCoinCount(0, requiredCoins) если CoinCounter найден
		if (_coinCounter != null)
		{
			_coinCounter.UpdateCoinCount(0, requiredCoins);
			GD.Print("[Ready] CoinCounter.UpdateCoinCount(0, requiredCoins) вызван");
		}
	}
	
	public void _on_area_2d_body_entered(Node body)
	{
		if (body is Character)
		{
			GD.Print("Персонаж вошёл в область монеты.");
			if (GameManager.Instance?.Profile != null)
			{
				bool added = GameManager.Instance.Profile.AddItem("Coin");
				GD.Print($"Монета добавлена в инвентарь: {added}");

				int requiredCoins = _baseLevel != null ? _baseLevel.RequiredCoins : 0;
				GD.Print($"RequiredCoins на уровне: {requiredCoins}");

				if (_coinCounter != null)
				{
					int coinCount = GameManager.Instance.Profile.GetAllItems().TryGetValue("Coin", out var value) ? value : 0;
					GD.Print($"Текущее количество монет: {coinCount}");
					_coinCounter.UpdateCoinCount(coinCount, requiredCoins);
				}
				else
				{
					GD.PrintErr("CoinCounter не найден!");
				}
				QueueFree();
				GD.Print("Монета удалена из сцены.");
			}
			else
			{
				GD.PrintErr("GameManager.Instance или Profile == null");
			}
		}
	}
}
