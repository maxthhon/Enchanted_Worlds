using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Класс профиля игрока. Хранит персональные данные: имя, инвентарь, прогресс и т.п.
/// Добавлены удобные методы управления данными с валидацией и логикой.
/// </summary>
public partial class PlayerProfile : Node
{
	public string PlayerName { get; set; } = "Player";

	// Список ID уровней, которые были завершены игроком
	public List<string> CompletedLevels { get; set; } = new List<string>();

	// Локальный инвентарь (ограниченный контекст — например, только текущий уровень)
	public Dictionary<string, int> LocalInventory { get; set; } = new Dictionary<string, int>();

	// Глобальный инвентарь (вещи, которые остаются между уровнями)
	public Dictionary<string, int> GlobalInventory { get; set; } = new Dictionary<string, int>();

	/// <summary>
	/// Добавляет предмет в указанный инвентарь.
	/// Проверяет, существует ли такой предмет в базе данных.
	/// </summary>
	public bool AddItem(string itemId, int amount = 1, bool global = false)
	{
		if (!ItemDatabase.IsValidItem(itemId) || amount <= 0)
		{
			GD.PrintErr($"Попытка добавить недопустимый предмет: {itemId}");
			return false;
		}

		var inventory = global ? GlobalInventory : LocalInventory;

		if (inventory.ContainsKey(itemId))
			inventory[itemId] += amount;
		else
			inventory[itemId] = amount;
		GD.Print($"Объект добавлен {itemId}");
		return true;
	}

	/// <summary>
	/// Удаляет предмет из инвентаря, если хватает количества.
	/// </summary>
	public bool RemoveItem(string itemId, int amount = 1, bool global = false)
	{
		var inventory = global ? GlobalInventory : LocalInventory;

		if (!inventory.ContainsKey(itemId))
		{
			GD.PrintErr($"Попытка удалить несуществующий предмет: {itemId}");
			return false;
		}

		if (inventory[itemId] < amount)
		{
			GD.PrintErr($"Недостаточно предметов для удаления: {itemId}");
			return false;
		}

		inventory[itemId] -= amount;

		if (inventory[itemId] <= 0)
			inventory.Remove(itemId);

		return true;
	}

	/// <summary>
	/// Помечает уровень как завершённый. Повторные записи игнорируются.
	/// </summary>
	public void MarkLevelCompleted(string levelId)
	{
		if (!CompletedLevels.Contains(levelId))
		{
			CompletedLevels.Add(levelId);
			GD.Print($"Уровень пройден: {levelId}");
		}
	}

	/// <summary>
	/// Проверяет, завершал ли игрок указанный уровень.
	/// </summary>
	public bool HasCompletedLevel(string levelId)
	{
		return CompletedLevels.Contains(levelId);
	}

	/// <summary>
	/// Возвращает список всех пройденных уровней (копия).
	/// </summary>
	public List<string> GetCompletedLevels()
	{
		return new List<string>(CompletedLevels);
	}
	
	/// <summary>
	/// Возвращает копию словаря всех предметов из выбранного инвентаря.
	/// Это удобно для отображения в UI или сериализации.
	/// </summary>
	public Dictionary<string, int> GetAllItems(bool global = false)
	{
		var source = global ? GlobalInventory : LocalInventory;
		return new Dictionary<string, int>(source); // Копия, чтобы избежать прямого изменения извне
	}

	/// <summary>
	/// Проверяет, есть ли в инвентаре указанный предмет и в достаточном количестве.
	/// </summary>
	public bool HasItem(string itemId, int count = 1, bool global = false)
	{
		var source = global ? GlobalInventory : LocalInventory;

		return source.TryGetValue(itemId, out int currentCount) && currentCount >= count;
	}
}
