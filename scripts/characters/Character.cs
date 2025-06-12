using Godot;
using System;

public partial class Character : CharacterBody2D
{
	[Export] public int health = 100;
	[Export] public int maxHealth = 100;

	public bool isInterupted = false;
	[Export] public float maxSpeed = 50;
	[Export] public float acceleration = 400;
	[Export] public float deceleration = 600;

	private Vector2 inputDirection = Vector2.Zero;
	private AnimatedSprite2D animation;

	private bool interruptRequested = false;
	private float interruptCooldown = 0f;
	private const float InterruptCooldownTime = 0.3f; // Кулдаун в секундах

	public override void _Ready()
	{
		animation = GetNode<AnimatedSprite2D>("CharAnimation");
		var camera = GetNode<Camera2D>("CharCamera");

		camera.Zoom = new Vector2(0.7f, 0.7f);

		camera.DragHorizontalEnabled = true;
		camera.DragVerticalEnabled = true;

		camera.DragLeftMargin = 0.25f;
		camera.DragRightMargin = 0.25f;
		camera.DragTopMargin = 0.2f;
		camera.DragBottomMargin = 0.2f;

		camera.PositionSmoothingEnabled = true;
		camera.PositionSmoothingSpeed = 5.0f;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (interruptCooldown > 0f)
			interruptCooldown -= (float)delta;

		HandleInput((float)delta);
		MoveAndSlide();

		//// Сохраняем позицию игрока каждый кадр
		//try
		//{
			//GameManager.Instance.Profile.SavedPlayerPosition = Position;
		//}
		//catch (Exception ex)
		//{
			//GD.PrintErr($"Ошибка сохранения позиции игрока: {ex.Message}");
		//}
	}

	private void HandleInput(float delta)
	{
		inputDirection = Input.GetVector("left", "right", "up", "down").Normalized();

		if (Input.IsActionJustPressed("interaction") && interruptCooldown <= 0f)
		{
			interruptRequested = true;
			interruptCooldown = InterruptCooldownTime;
		}
		else
		{
			isInterupted = false;
			//GD.Print("interrupt" + isInterupted);
		}

		if (inputDirection != Vector2.Zero)
		{
			float currentMaxSpeed = Input.IsActionPressed("shift") ? maxSpeed * 1.7f : maxSpeed;
			float speedScale = Input.IsActionPressed("shift") ? 2.0f : 1.5f;

			Velocity += inputDirection * acceleration * delta;
			Velocity = Velocity.LimitLength(currentMaxSpeed);
			animation.SpeedScale = speedScale;

			UpdateAnimation(inputDirection);
		}
		else
		{
			Velocity = Velocity.MoveToward(Vector2.Zero, deceleration * delta);
			animation.Play("stage");
		}

		//if (Input.IsActionJustPressed("ui_select"))
		//{
			//Attack();
		//}
	}

	private void UpdateAnimation(Vector2 direction)
	{
		if (direction.Y > 0 && direction.X == 0)
		{
			animation.Play("walk_down");
		}
		else if (direction.Y < 0 && direction.X == 0)
		{
			animation.Play("walk_up");
		}
		else if (direction.X < 0)
		{
			animation.FlipH = true;
			animation.Play(direction.Y < 0 ? "walk_right_up" : "walk_right_down");
		}
		else if (direction.X > 0)
		{
			animation.FlipH = false;
			animation.Play(direction.Y < 0 ? "walk_right_up" : "walk_right_down");
		}
	}

	private void Attack()
	{
		GD.Print("Attack triggered!");
		// Добавь логику атаки тут
	}

	public void _on_player_hit_box_body_entered(Node2D body)
	{
		// Реакция на столкновение
	}

	public void _on_player_hit_box_body_exited(Node2D body)
	{
		// Реакция на выход из зоны
	}

	public async void ShowTooltip(string text, float seconds, bool forceShow = false)
	{
		GD.Print($"Вопрос посещён {text}");
		var tooltip = GetNodeOrNull<CanvasLayer>("Tooltip");
		if (tooltip == null)
			return;

		var colorRect = tooltip.GetNode<ColorRect>("ColorRect");
		var label = colorRect.GetNode<Label>("Label");
		label.Text = text;

		tooltip.Visible = true;

		// Просто показываем и скрываем без плавной анимации
		colorRect.Modulate = new Color(colorRect.Modulate, 1f);

		await ToSignal(GetTree().CreateTimer(seconds), "timeout");

		colorRect.Modulate = new Color(colorRect.Modulate, 0f);
		tooltip.Visible = false;
	}

	public bool TryInterrupt()
	{
		if (interruptRequested)
		{
			interruptRequested = false;
			return true;
		}
		return false;
	}
}
