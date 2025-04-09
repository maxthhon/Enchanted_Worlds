using Godot;
using System;
using System.IO;
using System.Text.Json;

public partial class TileSetter : Node2D
{
	[Export] public PackedScene GrassTileScene { get; set; }
	[Export] public string JsonFilePath = "res://grass_positions.json"; // Путь к JSON файлу
	private const int GridSize = 32; // Размер ячейки сетки

	public override void _Ready()
	{
		LoadGrassTilesFromJson();
	}

	private void LoadGrassTilesFromJson()
	{
		try
		{
			// Чтение JSON файла
			string jsonString = File.ReadAllText(ProjectSettings.GlobalizePath(JsonFilePath));
			GD.Print("JSON Content: " + jsonString);
			GrassGridData gridData = JsonSerializer.Deserialize<GrassGridData>(jsonString);

			if (gridData == null)
			{
				GD.PrintErr("Deserialized GrassGridData is null.");
				return;
			}

			if (gridData.grid == null)
			{
				GD.PrintErr("Grid data in GrassGridData is null.");
				return;
			}

			int n = gridData.n; // Количество строк
			int m = gridData.m; // Количество столбцов

			// Перебор сетки и создание тайлов
			for (int i = 0; i < n; i++)
			{
				for (int j = 0; j < m; j++)
				{
					if (gridData.grid[i][j] == 1) // Если в ячейке 1, создаем тайл
					{
						float x = (j * GridSize); // Центрируем тайл
						float y = (i * GridSize) + 16; // Центрируем тайл

						var grassTile = GrassTileScene.Instantiate<Node2D>();
						grassTile.Position = new Vector2(x, y);
						AddChild(grassTile);
					}
				}
			}
		}
		catch (Exception e)
		{
			GD.PrintErr("Error loading grass tile positions from JSON: " + e.Message);
		}
	}

	// Класс для представления данных из JSON
	private class GrassGridData
	{
		public int n { get; set; } // Количество строк
		public int m { get; set; } // Количество столбцов
		public int[][] grid { get; set; } // Двумерный массив с данными о тайлах
	}
}
