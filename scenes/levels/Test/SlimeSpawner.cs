using Godot;
using System;

public partial class SlimeSpawner : Node
{
	[Export]
	public PackedScene SlimeScene;
	[Export]
	public float SpawnInterval = 2.0f;

	public Vector2 MinSpawnPosition = new Vector2(0, 720);

	public Vector2 MaxSpawnPosition = new Vector2(1280, 750);

	private float _timeSinceLastSpawn = 0.0f;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (SlimeScene == null)
		{
			GD.PrintErr("SlimeScene is not assigned!");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		_timeSinceLastSpawn += (float)delta;

		if (_timeSinceLastSpawn >= SpawnInterval && SlimeScene != null)
		{
			SpawnSlime();
			_timeSinceLastSpawn = 0.0f;
		}
	}

	private void SpawnSlime()
	{
		Node2D slimeInstance = (Node2D)SlimeScene.Instantiate();
		AddChild(slimeInstance);

		// Set the position of the slime instance to a random position within the specified bounds
		float randomX = (float)GD.RandRange(MinSpawnPosition.X, MaxSpawnPosition.X);
		float randomY = (float)GD.RandRange(MinSpawnPosition.Y, MaxSpawnPosition.Y);
		slimeInstance.Position = new Vector2(randomX, randomY);
	}
}
