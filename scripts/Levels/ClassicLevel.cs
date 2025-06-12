using Godot;
using System;

public partial class ClassicLevel : BaseLevel
{
	[Export]
	public int CoinsOnLevel;
	
	[Export]
	public int CurrentLevel;
	
	[Export]
	public string LevelSong = "LibraryTheme_NewAdventure";
	
	public override void _Ready()
	{
		base._Ready();
		RequiredCoins = CoinsOnLevel; // Например, для этого уровня нужно 6 монет
		NextLevelPath = GameManager.Instance.Profile.GetScenePathById(CurrentLevel + 1);
		GD.Print($"Level{CurrentLevel} is ready!");
		GetNode<MusicController>("/root/Music").PlayTrackByName(LevelSong);
	}

	public override void PauseGame()
	{
		base.PauseGame();
		GD.Print($"Game paused in Level {CurrentLevel}.");
	}

	public override void ResumeGame()
	{
		base.ResumeGame();
		GD.Print($"Game resumed in Level {CurrentLevel}.");
	}
}
