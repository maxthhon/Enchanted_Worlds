using Godot;
using System;

public partial class Character : CharacterBody2D
{
	[Export] public float max_speed = 75;
	[Export] public float acceleration = 400;
	[Export] public float deceleration = 600;

	[Export] public Vector2 level_size = new Vector2(1280, 720);
	private bool damage_is_taked = false;
	
	public float min_zoom = 2.0f;
	public float max_zoom = 5.0f;

	private static readonly Vector2 LEFT = new Vector2(-1, 0);
	private static readonly Vector2 RIGHT = new Vector2(1, 0);
	private static readonly Vector2 UP = new Vector2(0, -1);
	private static readonly Vector2 DOWN = new Vector2(0, 1);
	private static readonly Vector2 ZERO = Vector2.Zero;

	private void PlayerAnimations(Vector2 direction)
	{
		var animation = GetNode<AnimatedSprite2D>("Char_Ani");

		if (!damage_is_taked)
		{
			if (direction != ZERO)
			{
				if (direction == LEFT) 
					{
					animation.Play("run_left");
					}
				else if (direction == RIGHT)
					{
					animation.Play("run_right");
					}
				else if (direction == UP)
					animation.Play("run_up");
				else if (direction == DOWN)
					animation.Play("run_down");
			}
			else
			{
				animation.Play("stage");
			}
		}
		else
		{
			animation.Play("atack");
		}
	}

	private void GetInput()
	{
		Vector2 input_direction = Input.GetVector("left", "right", "up", "down");
		PlayerAnimations(input_direction);
		
		if (input_direction != Vector2.Zero)
		{
			
			if(Input.IsActionPressed("shift"))
			{
			Velocity += input_direction * acceleration * (float)GetProcessDeltaTime();
			Velocity = Velocity.LimitLength(max_speed * 1.7f);
			}
			else
			{
			Velocity += input_direction * acceleration * (float)GetProcessDeltaTime();
			Velocity = Velocity.LimitLength(max_speed);
			}
		}
		else
		{
			// Decelerate
			Velocity = Velocity.MoveToward(Vector2.Zero, deceleration * (float)GetProcessDeltaTime());
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


	private void _on_headdown_timeout()
	{
		damage_is_taked = false;
	}
}
