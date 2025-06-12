using Godot;
using System;

public partial class DangerTile : StaticBody2D
{
	private AnimatedSprite2D _animSprite;
	private CollisionShape2D _collision;
	private const string DangerAnimation = "default";

	public override void _Ready()
	{
		GD.Print("DangerTile _Ready called");
		_animSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
		_collision = GetNode<CollisionShape2D>("CollisionShape2D");

		// Вывести все имена анимаций
		foreach (var anim in _animSprite.SpriteFrames.GetAnimationNames())
			GD.Print($"Available animation: {anim}");

		// Убедиться, что тайл в начальном состоянии
		_collision.Disabled = true;
		_animSprite.Play(DangerAnimation);
		_animSprite.Frame = 0; // Вернуть анимацию на первый кадр
		_animSprite.Stop();
		GD.Print("DangerTile initialized: collision enabled, animation reset");

		_animSprite.FrameChanged += _on_animated_sprite_2d_frame_changed;
	}

	private void _on_animated_sprite_2d_frame_changed()
	{
		// Если дошли до последнего кадра DangerAnimation
		if (_animSprite.Animation == DangerAnimation && _animSprite.Frame == 0)
		{
			_collision.Disabled = false;
			_animSprite.Stop();
			_animSprite.Frame = 5;
			GD.Print("Animation reached the last frame, collision enabled");
		}
	}

	private void _on_area_2d_body_exited(Node2D body)
	{
		GD.Print($"Body exited: {body.Name}, type: {body.GetType()}");
		if (body is Character)
		{
			GD.Print("Body is Character, triggering animation and disabling collision");
			_collision.Disabled = true;
			_animSprite.Play(DangerAnimation);
			// Не перематывайте и не останавливайте здесь!
		}
		else
		{
			GD.Print("Body is NOT Character, ignoring");
		}
	}
}
