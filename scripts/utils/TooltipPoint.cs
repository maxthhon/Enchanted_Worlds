using Godot;
using System;

public partial class TooltipPoint : StaticBody2D
{
	private AnimatedSprite2D _animationPlayer;
	[Export] public string TooltipText = "Это точка подсказки!";
	public float TooltipTime = 0.5f;

	private Character _currentCharacter = null;

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_animationPlayer.Play();

		var area = GetNode<Area2D>("Area2D");
		area.BodyEntered += OnBodyEntered;
		area.BodyExited += OnBodyExited;
	}

	private async void OnBodyEntered(Node body)
	{
		GD.Print("Вопрос посещён");
		if (body is Character character)
		{
			_currentCharacter = character;
			while (_currentCharacter != null)
			{
				character.ShowTooltip(TooltipText, TooltipTime, false); // не форсируем исчезновение
				await ToSignal(GetTree().CreateTimer(TooltipTime), "timeout");
			}
		}
	}

	private void OnBodyExited(Node body)
	{
		if (body is Character character && character == _currentCharacter)
		{
			_currentCharacter = null;
			// Принудительно скрываем подсказку
			character.ShowTooltip("", 0.01f, true);
		}
	}
}
