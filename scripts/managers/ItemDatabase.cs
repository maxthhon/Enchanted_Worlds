using Godot;
using System;
using System.Collections.Generic;

/// <summary>
/// Централизованная база данных игровых предметов.
/// Позволяет единообразно управлять всеми доступными предметами в игре.
/// Хорошая практика — выносить такие вещи в отдельную структуру для удобства поддержки и масштабирования.
/// </summary>
public static class ItemDatabase
{
	// Основной словарь всех предметов, где ключ — это ID предмета (используемый в коде и сохранениях),
	// а значение — объект с данными о предмете.
	public static readonly Dictionary<string, ItemData> Items = new Dictionary<string, ItemData>
	{
		{ "GoldKey", new ItemData("GoldKey", "Золотой Ключ", "Окрывает золотой замок.", ItemType.Quest) },
		{ "SilverKey", new ItemData("SilverKey", "Серебряный Ключ", "Окрывает серебрянный замок.", ItemType.Quest) },
		{ "BronzeKey", new ItemData("BronzeKey", "Бронзовый Ключ", "Окрывает бронзовый замок.", ItemType.Quest) },
		{ "Coin", new ItemData("Coin", "Золотая монетка", "Блестит очень красиво.", ItemType.Misc) },
		// В дальнейшем легко расширять этот список, добавляя новые предметы.
	};

	/// <summary>
	/// Проверка, существует ли предмет с указанным ID в базе.
	/// Используется перед добавлением или отображением предмета, чтобы избежать ошибок.
	/// </summary>
	public static bool IsValidItem(string itemId)
	{
		return Items.ContainsKey(itemId);
	}

	/// <summary>
	/// Получить данные о предмете по его ID.
	/// Возвращает null, если предмет не найден — это удобно при использовании TryGet.
	/// </summary>
	public static ItemData GetItem(string itemId)
	{
		return Items.TryGetValue(itemId, out var item) ? item : null;
	}
}

/// <summary>
/// Категории предметов. Позволяет логически группировать предметы по их типу.
/// Это полезно, например, для фильтрации в инвентаре или определения механики использования.
/// </summary>
public enum ItemType
{
	Weapon,
	Armor,
	Consumable,
	Quest,
	Misc // Для всего остального
}

/// <summary>
/// Структура, представляющая собой описание предмета.
/// Можно дополнять другими свойствами: цена, путь к иконке, редкость, ограничения по использованию и т.п.
/// </summary>
public class ItemData
{
	public string Id { get; }
	public string Name { get; }
	public string Description { get; }
	public ItemType Type { get; }

	public ItemData(string id, string name, string description, ItemType type)
	{
		Id = id;
		Name = name;
		Description = description;
		Type = type;
	}
}
