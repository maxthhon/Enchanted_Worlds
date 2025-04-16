using Godot;
using System;

public partial class PlayerSpawner : Node2D
{
	[Export]
	public PackedScene PlayerScene { get; set; } // Ссылка на сцену игрока

	//[Export]
	//public Vector2 SpawnPosition { get; set; } = Vector2.Zero; // Позиция спауна

	public override void _Ready()
	{
		if (PlayerScene == null)
		{
			GD.PrintErr("PlayerScene is not assigned!");
			return;
		}

		CharacterBody2D playerInstance = PlayerScene.Instantiate<CharacterBody2D>();
		playerInstance.Position = GlobalPosition;

		GetParent().CallDeferred("add_child", playerInstance);
	}

}
