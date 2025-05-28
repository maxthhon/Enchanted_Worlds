using Godot;
using System;
using System.Text.Json;

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }

	public PlayerProfile Profile { get; private set; } = new PlayerProfile();

	private const string SavePath = "user://savegame.json";

	public override void _Ready()
	{
		Instance = this;
		LoadProfile();
	}

	public void SaveProfile()
	{
		string json = JsonSerializer.Serialize(Profile, new JsonSerializerOptions
		{
			WriteIndented = true
		});

		using var file = Godot.FileAccess.Open(SavePath, Godot.FileAccess.ModeFlags.Write);
		file.StoreString(json);

		GD.Print("Профиль сохранён.");
	}

	public void LoadProfile()
	{
		if (!Godot.FileAccess.FileExists(SavePath))
		{
			GD.Print("Файл сохранения не найден. Новый профиль создан.");
			return;
		}

		using var file = Godot.FileAccess.Open(SavePath, Godot.FileAccess.ModeFlags.Read);
		string json = file.GetAsText();

		try
		{
			Profile = JsonSerializer.Deserialize<PlayerProfile>(json) ?? new PlayerProfile();
			GD.Print("Профиль загружен.");
		}
		catch (Exception ex)
		{
			GD.PrintErr($"Ошибка загрузки профиля: {ex.Message}");
			Profile = new PlayerProfile(); // fallback
		}
	}
	
	public void ExitLevel()
	{
		GetTree().ChangeSceneToFile("res://scenes/main/MainMenu.tscn");
	}
}
