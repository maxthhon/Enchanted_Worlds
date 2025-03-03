using Godot;
using System;

public partial class Tower : StaticBody2D
{
	private int health = 100;
	private int maxHealth = 100;
	private bool towerIsAlive = true;

	public override void _Ready()
	{
		GetNode<TextureProgressBar>("HealthBar").Value = health;
	}

	public void Die()
	{
		towerIsAlive = false;
		//GetNode<Label>("/root/Node2D/CharacterBody2D/Label2").Text = "(отладка) ХАХАХАХХАХАХАХХАХАХХА";
	}

	public void TakeDamage(int amount)
	{
		health -= amount;
		GetNode<TextureProgressBar>("HealthBar").Value = health;
		if (health <= 0)
		{
			Die();
		}
	}

	public void Heal(int amount)
	{
		health = Math.Min(health + amount, maxHealth);
		// g.tower_health = health; // Это нужно заменить на актуальный способ доступа к глобальным переменным в вашем проекте
		GetNode<TextureProgressBar>("HealthBar").Value = health;
	}

	public void _on_hitbox_body_entered(Node body)
	{
		//take damage
	}
}
