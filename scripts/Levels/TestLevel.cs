using Godot;
using System;

public partial class TestLevel : BaseLevel
{
	public override void _Ready()
	{
		base._Ready();
		RequiredCoins = 5; // Например, для этого уровня нужно 5 монет
		NextLevelPath = "res://scenes/main/MainMenu.tscn";
		GetNode<MusicController>("/root/Music").PlayTrackByName("LibraryTheme_NewAdventure");
		GD.Print("TestLevel is ready!");
		// Можно добавить дополнительные инициализации
	}

	public override void PauseGame()
	{
		base.PauseGame(); // сохраняем базовую реализацию
		GD.Print("Game paused in Level TEST.");
	}

	public override void ResumeGame()
	{
		base.ResumeGame(); // сохраняем базовую реализацию
		GD.Print("Game resumed in Level TEST.");
	}
}
