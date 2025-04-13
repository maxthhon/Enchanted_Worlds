using Godot;
using System;

public partial class PlayerSpawner : Node2D
{
	[Export] public PackedScene PlayerScene { get; set; } // Reference to the TileCharacter scene
	[Export] public Vector2 SpawnPosition { get; set; } = new Vector2(100, 100); // Default spawn position

	public override void _Ready()
	{
		SpawnPlayer();
	}

	private void SpawnPlayer()
	{
		if (PlayerScene == null)
		{
			GD.PrintErr("PlayerScene is not assigned in the editor.");
			return;
		}

		var player = PlayerScene.Instantiate<Node2D>();
		player.Position = SpawnPosition;
		AddChild(player);

		GD.Print("Player spawned at position: " + SpawnPosition);
	}
}
