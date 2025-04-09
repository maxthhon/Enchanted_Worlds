using Godot;
using System;

public partial class Grass2 : StaticBody2D
{
	[Export]
	public int offset = 3;
	
	private Timer timer;
	private Vector2 startPos;
	private Vector2 targetPos;
	private float timeElapsed;
	private float duration = 1.0f; // Длительность анимации
	
	public override void _Ready()
	{
		// Получаем таймер с именем "Timer"
		timer = GetNode<Timer>("Timer");
	}

	public void _on_tile_hit_box_area_entered(Area2D area)
	{
		// Инициализация для анимации при входе в область
		startPos = Position;
		targetPos = new Vector2(Position.X, Position.Y + offset); // Целевая позиция смещена по оси Y
		timeElapsed = 0.0f; // Обнуляем время
		timer.Start(duration); // Стартуем таймер на продолжительность анимации
	}

	public void _on_tile_hit_box_area_exited(Area2D area)
	{
		// Инициализация для анимации при выходе из области
		startPos = Position;
		targetPos = new Vector2(Position.X, Position.Y - offset); // Целевая позиция смещена по оси Y
		timeElapsed = 0.0f; // Обнуляем время
		timer.Start(duration); // Стартуем таймер на продолжительность анимации
	}

	private void _on_anumation_frame_timeout()
	{
		// Завершаем таймер, если прошло нужное время
		if (timeElapsed >= duration)
		{
			timer.Stop();
			return;
		}

		// Интерполируем позицию с использованием экспоненциальной функции
		timeElapsed += (float)timer.WaitTime; // Увеличиваем время прошедшее с начала анимации
		float t = timeElapsed / duration; // Нормализуем время, чтобы оно варьировалось от 0 до 1

		// Применяем экспоненциальное затухание (easeInOut) для плавного перехода
		float easedT = Mathf.Pow(t, 3); // Экспоненциальная интерполяция

		// Обновляем позицию объекта
		Position = new Vector2(Position.X, Mathf.Lerp(startPos.Y, targetPos.Y, easedT));
	}
}
