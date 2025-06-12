using Godot;
using System;
using System.Text.Json;

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }

	public PlayerProfile Profile { get; private set; } = new PlayerProfile();

	private const string SavePath = "user://savegame.json";
	private Timer _autoSaveTimer;
	private const float AutoSaveInterval = 10f; // Сохранять каждые 10 секунд

	public override void _Ready()
	{
		Instance = this;
		LoadProfile();

		// --- Добавлено автосохранение ---
		_autoSaveTimer = new Timer();
		_autoSaveTimer.WaitTime = AutoSaveInterval;
		_autoSaveTimer.OneShot = false;
		_autoSaveTimer.Autostart = true;
		_autoSaveTimer.Timeout += () => SaveProfile();
		AddChild(_autoSaveTimer);
		// --- Конец добавления ---
	}

	public void SaveProfile()
	{
		try
		{
			string json = JsonSerializer.Serialize(Profile, new JsonSerializerOptions
			{
				WriteIndented = true
			});

			using var file = Godot.FileAccess.Open(SavePath, Godot.FileAccess.ModeFlags.Write);
			file.StoreString(json);

			GD.Print("Профиль сохранён.");
		}
		catch (Exception ex)
		{
			GD.PrintErr($"Ошибка сохранения профиля: {ex.Message}");
		}
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

	public void ResetProfile()
	{
		if (Godot.FileAccess.FileExists(SavePath))
		{
			using var dir = Godot.DirAccess.Open("user://");
			if (dir != null)
			{
				dir.Remove("savegame.json");
				GD.Print("Файл сохранения удалён.");
			}
		}
		Profile = new PlayerProfile();
		GD.Print("Профиль сброшен.");
	}

	public void OnLevelCompleted()
	{
		SaveProfile();
		GD.Print("Профиль сохранён после завершения уровня.");
	}
}
