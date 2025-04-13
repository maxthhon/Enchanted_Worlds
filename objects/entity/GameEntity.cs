using Godot;
using System;
using System.Collections.Generic; // Добавляем пространство имен для List

public abstract partial class GameEntity : CharacterBody2D
{
	[Export] public string EntityName = "Unnamed Entity";
	[Export] public int MaxHealth { get; protected set; } = 100;
	public int CurrentHealth { get; protected set; }

	public bool IsAlive => CurrentHealth > 0;

	// Добавляем инвентарь
	public List<string> Inventory { get; protected set; } = new List<string>();

	public override void _Ready()
	{
		CurrentHealth = MaxHealth;
		OnEntityReady();
	}

	protected virtual void OnEntityReady()
	{
		// Может быть переопределено в потомках
	}

	public virtual void TakeDamage(int amount)
	{
		CurrentHealth -= amount;
		GD.Print($"{EntityName} получил {amount} урона, осталось {CurrentHealth} HP");

		if (CurrentHealth <= 0)
		{
			Die();
		}
	}

	protected virtual void Die()
	{
		GD.Print($"{EntityName} погиб.");
		QueueFree();
	}

	public abstract void Interact(Node2D other);

	// Добавляем методы для работы с инвентарем
	public void AddItemToInventory(string item)
	{
		Inventory.Add(item);
		GD.Print($"{EntityName} добавил {item} в инвентарь.");
	}

	public void RemoveItemFromInventory(string item)
	{
		if (Inventory.Contains(item))
		{
			Inventory.Remove(item);
			GD.Print($"{EntityName} убрал {item} из инвентаря.");
		}
		else
		{
			GD.Print($"{EntityName} не имеет {item} в инвентаре.");
		}
	}

	public bool HasItemInInventory(string item)
	{
		return Inventory.Contains(item);
	}

	public void DisplayInventory()
	{
		GD.Print($"{EntityName} инвентарь:");
		if (Inventory.Count == 0)
		{
			GD.Print("Инвентарь пуст.");
		}
		else
		{
			foreach (string item in Inventory)
			{
				GD.Print($"- {item}");
			}
		}
	}
}
