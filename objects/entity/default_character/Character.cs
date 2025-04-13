using Godot;
using System;
using System.Collections.Generic;

public partial class Character : CharacterBody2D
{
	public float max_speed = 50;
	public float acceleration = 400;
	public float deceleration = 600;

	public Vector2 level_size = new Vector2(1280, 720);
	private bool damage_is_taked = false;
	
	private Random random = new Random();
	public float min_zoom = 2.0f;
	public float max_zoom = 5.0f;
	
	private List<Slime> enemieList = new List<Slime>(); // Initialize the list

	private static readonly Vector2 LEFT = new Vector2(-1, 0);
	private static readonly Vector2 RIGHT = new Vector2(1, 0);
	private static readonly Vector2 UP = new Vector2(0, -1);
	private static readonly Vector2 DOWN = new Vector2(0, 1);
	private static readonly Vector2 ZERO = Vector2.Zero;

	private Tower tower;
	private Slime slime; // Declare slime as a global variable

	private void PlayerAnimations(Vector2 direction)
	{
		var animation = GetNode<AnimatedSprite2D>("Char_Ani");
		if (!damage_is_taked)
		{
			if (direction != ZERO)
			{
				if (direction == LEFT) 
					{
					animation.Play("walk_left");
					}
				else if (direction == RIGHT)
					{
					animation.Play("walk_right");
					}
				else if (direction == UP)
					animation.Play("walk_up");
				else if (direction == DOWN)
					animation.Play("walk_down");
			}
			else
			{
				animation.Play("stage");
			}
		}
		else
		{
			animation.Play("stage");
		}
	}

	private void GetInput()
	{
		var animation = GetNode<AnimatedSprite2D>("Char_Ani");
		Vector2 input_direction = Input.GetVector("left", "right", "up", "down");
		PlayerAnimations(input_direction);
		
		if (input_direction != Vector2.Zero)
		{
			
			if(Input.IsActionPressed("shift"))
			{
			Velocity += input_direction * acceleration * (float)GetProcessDeltaTime();
			Velocity = Velocity.LimitLength(max_speed * 1.7f);
			animation.SpeedScale = 2.0f;
			}
			else
			{
			Velocity += input_direction * acceleration * (float)GetProcessDeltaTime();
			Velocity = Velocity.LimitLength(max_speed);
			animation.SpeedScale = 1.5f;
			}
		}
		else
		{
			// Decelerate
			Velocity = Velocity.MoveToward(Vector2.Zero, deceleration * (float)GetProcessDeltaTime());
		}

		if (Input.IsActionJustPressed("ui_select")) // Проверка нажатия пробела
		{
			Attack();
		}
	}
	
	private void SetCameraZoom()
	{
		var camera = GetNode<Camera2D>("CharCamera");
		// Определяем расстояние до границ уровня
		float distanceToLeft = Position.X;
		float distanceToRight = level_size.X - Position.X;
		float distanceToTop = Position.Y;
		float distanceToBottom = level_size.Y - Position.Y;

		// Находим минимальное расстояние до границы
		float minDistance = Mathf.Min(Mathf.Min(distanceToLeft, distanceToRight), Mathf.Min(distanceToTop, distanceToBottom));

		// Устанавливаем зум в зависимости от расстояния
		// Используем экспоненциальную функцию для расчета zoomFactor
		float t = minDistance / 100.0f; // Нормализуем расстояние
		t = Mathf.Clamp(t, 0, 1); // Ограничиваем значение от 0 до 1

		// Применяем экспоненциальную функцию
		float zoomFactor = Mathf.Lerp(max_zoom, min_zoom, Mathf.Pow(t, 2.71828f)); // exponent - это степень, которую вы можете настроить

		camera.Zoom = new Vector2(zoomFactor, zoomFactor);
	}

	public override void _Ready()
	{
		tower = GetParent().GetNode<Tower>("Tower");
		var slimeSpawner = GetParent().GetNode<SlimeSpawner>("SlimeSpawner");
	}

	public override void _PhysicsProcess(double delta)
	{
		GetInput();
		SetCameraZoom();
		MoveAndSlide();

		// Удерживаем персонажа в границах уровня
		Position = new Vector2(
			Mathf.Clamp(Position.X, 0, level_size.X),
			Mathf.Clamp(Position.Y, 0, level_size.Y)
		);
	}
	
	public void _on_player_hit_box_body_entered(Node2D body) 
	{
		if (body is Slime)
		{
			slime = (Slime)body;
			enemieList.Add(slime);
		}
		else if (body is Tower)
		{
			int health = random.Next(1, 5);
			tower.Heal(health); // Уменьшает здоровье башни на 10 единиц
		}
	}
	
	public void _on_player_hit_box_body_exited(Node2D body)
	{
		if (body is Slime)
		{
			slime = (Slime)body;
			enemieList.Remove(slime);
		}
	}

	private void Attack()
	{
		foreach (var enemie in enemieList)
		{
			enemie.TakeDamage(2); // Наносим урон всем слаймам в списке
		}
	}
}
