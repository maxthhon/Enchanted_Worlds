using Godot;
using System;

public partial class Slime : CharacterBody2D
{
	private int maxSpeed;
	private int acceleration;
	private int deceleration = 100;

	private int health;
	private const int maxHealth = 7;
	private bool slimeIsAlive = true;
	private bool slimeRun = true;
	private Random random = new Random();
	
	private Node2D target;
	private Tower tower;
	
	public override void _Ready()
	{
		var animatedSprite = GetNode<AnimatedSprite2D>("Slime_Ani");
		animatedSprite.Play("walk");
		target = GetParent().GetParent().GetNode<Node2D>("Tower");
		tower = GetParent().GetParent().GetNode<Tower>("Tower");
		
		GD.Print(tower);

		maxSpeed = random.Next(20, 91); // randi_range(20, 90)
		acceleration = random.Next(15, 76); // randi_range(15, 75)
		health = random.Next(5, 8); // randi_range(5, 7)
		
		GetNode<TextureProgressBar>("HealthBar").Value = health;
		GetNode<TextureProgressBar>("HealthBar").MaxValue = maxHealth;
	}

	public override void _PhysicsProcess(double delta)
	{
		var animation = GetNode<AnimatedSprite2D>("Slime_Ani");
		
		Vector2 towerPosition = target.GlobalPosition; 
		var direction = (towerPosition - GlobalPosition - new Vector2(0, -30)).Normalized();
		Velocity = direction * 40;
		
		if (!tower.isAlive()) {
			animation.Animation = "stage";
			return;
		}
		
		if (animation.Frame >= 3 && animation.Frame <= 6)
		{
			MoveAndSlide();
		}
	}
	
	public void _on_slime_hitbox_body_entered(Node2D body) 
	{
		if (body is Tower)
		{
			int damage = random.Next(5, 15);
			tower.TakeDamage(damage); // Уменьшает здоровье башни на 10 единиц
		}
	}
	
	private void Die()
	{
		GetNode<AnimatedSprite2D>("Slime_Ani").Play("die");
	}
	
	public void _on_slime_ani_frame_changed()
	{
		var animatedSprite = GetNode<AnimatedSprite2D>("Slime_Ani");
		if (animatedSprite.Animation == "die" && animatedSprite.Frame >= 7)
		{
			QueueFree();
		}
	}

	public void TakeDamage(int amount)
	{
		if (slimeIsAlive)
		{
			health -= amount;
			GetNode<TextureProgressBar>("HealthBar").Value = health;
			if (health <= 0)
			{
				slimeIsAlive = false;
				Die();
			}
		}
	}

	private void Heal(int amount)
	{
		health = Math.Min(health + amount, maxHealth);
	}
}
