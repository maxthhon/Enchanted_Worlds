using Godot;
using System;

public partial class Coin : StaticBody2D
{
	private AnimatedSprite2D _animationPlayer;

	public override void _Ready()
	{
		_animationPlayer = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_animationPlayer.Play();
	}
	
	public void _on_area_2d_body_entered(Node body)
	{
		if (body is Character)
		{
			// Убедись, что GameManager.Instance и Profile НЕ null
			if (GameManager.Instance?.Profile != null)
			{
				GameManager.Instance.Profile.AddItem("Coin");
				QueueFree();
			}
			else
			{
				GD.PrintErr("GameManager.Instance или Profile == null");
			}
		}
	}
}
