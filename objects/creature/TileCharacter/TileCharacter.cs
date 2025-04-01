using Godot;
using System;
using System.Collections.Generic;

public partial class TileCharacter : CharacterBody2D
{
	// Добавленные параметры сетки
	[Export] public int TileSize { get; set; } = 32;
	private Vector2 _targetPosition;
	private bool _isMoving;
	
	// Остальные существующие переменные
	public float max_speed = 50;
	public float acceleration = 400;
	public float deceleration = 600;
	public Vector2 level_size = new Vector2(1280, 720);
	private bool damage_is_taked = false;
	private Random random = new Random();
	public float min_zoom = 2.0f;
	public float max_zoom = 5.0f;

	private static readonly Vector2 LEFT = new Vector2(-1, 0);
	private static readonly Vector2 RIGHT = new Vector2(1, 0);
	private static readonly Vector2 UP = new Vector2(0, -1);
	private static readonly Vector2 DOWN = new Vector2(0, 1);
	private static readonly Vector2 ZERO = Vector2.Zero;

	public override void _Ready()
	{
		// Привязка начальной позиции к сетке
		Position = Position.Snapped(Vector2.One * TileSize);
		_targetPosition = Position;
	}

	private void GetInput()
	{
		var animation = GetNode<AnimatedSprite2D>("Char_Ani");
		Vector2 input_direction = Input.GetVector("left", "right", "up", "down");

		// Проверяем, если персонаж не движется
		if (!_isMoving && input_direction != Vector2.Zero)
		{
			// Привязываем текущую позицию к сетке
			Position = Position.Snapped(Vector2.One * TileSize);

			// Устанавливаем целевую позицию
			_targetPosition = Position + input_direction * TileSize;
			_isMoving = true;

			// Анимация движения
			PlayerAnimations(input_direction);
		}

		// Если персонаж движется, продолжаем движение к целевой позиции
		if (_isMoving)
		{
			Velocity = Position.DirectionTo(_targetPosition) * max_speed;

			if (Position.DistanceTo(_targetPosition) < 1f)
			{
				Position = _targetPosition;
				Velocity = Vector2.Zero;
				_isMoving = false; // Останавливаем движение

				// Переключаем анимацию на stage
				animation.Play("stage");
			}
		}
		else
		{
			Velocity = Vector2.Zero;
		}

		if (Input.IsActionJustPressed("ui_select")) // Проверка нажатия пробела
		{
			//пробел
		}
	}

	private void PlayerAnimations(Vector2 direction)
	{
		var animation = GetNode<AnimatedSprite2D>("Char_Ani");
		if (!damage_is_taked)
		{
			if (direction != ZERO)
			{
				if (direction == LEFT) 
					animation.Play("walk_left");
				else if (direction == RIGHT)
					animation.Play("walk_right");
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

	private void SetCameraZoom()
	{
		var camera = GetNode<Camera2D>("CharCamera");
		float distanceToLeft = Position.X;
		float distanceToRight = level_size.X - Position.X;
		float distanceToTop = Position.Y;
		float distanceToBottom = level_size.Y - Position.Y;

		float minDistance = Mathf.Min(Mathf.Min(distanceToLeft, distanceToRight), Mathf.Min(distanceToTop, distanceToBottom));

		float t = minDistance / 100.0f;
		t = Mathf.Clamp(t, 0, 1);

		float zoomFactor = Mathf.Lerp(max_zoom, min_zoom, Mathf.Pow(t, 2.71828f));
		camera.Zoom = new Vector2(zoomFactor, zoomFactor);
	}

	public void _on_player_hit_box_body_entered(Node2D body) 
	{
	
	}
	
	public void _on_player_hit_box_body_exited(Node2D body)
	{

	}
}
