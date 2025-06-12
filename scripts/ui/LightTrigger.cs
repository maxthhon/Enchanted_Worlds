using Godot;
using System;

public partial class LightTrigger : StaticBody2D
{
	[Export] public Color TargetColor { get; set; } = new Color(0, 0, 0, 1);
	[Export] public float TransitionSpeed { get; set; } = 2.0f; // Скорость перехода

	private CanvasModulate _canvasModulate;
	private bool _isTransitioning = false;
	private float _transitionProgress = 0f;
	private Color _startColor;

	public override void _Ready()
	{
		try
		{
			_canvasModulate = GetParent().GetNode<CanvasModulate>("CanvasModulate");
		}
		catch (Exception ex)
		{
			GD.PrintErr($"LightTrigger error (Ready): {ex.Message}");
		}
	}

	private void _on_area_2d_body_entered(Node body)
	{
		try
		{
			GD.Print($"canvasmodulate_triggered {TargetColor}");
			if (_canvasModulate != null)
			{
				_startColor = _canvasModulate.Color;
				_transitionProgress = 0f;
				_isTransitioning = true;
			}
		}
		catch (Exception ex)
		{
			GD.PrintErr($"LightTrigger error: {ex.Message}");
		}
	}

	public override void _Process(double delta)
	{
		if (_isTransitioning && _canvasModulate != null)
		{
			_transitionProgress += (float)delta * TransitionSpeed;
			if (_transitionProgress >= 1f)
			{
				_canvasModulate.Color = TargetColor;
				_isTransitioning = false;
			}
			else
			{
				_canvasModulate.Color = _startColor.Lerp(TargetColor, _transitionProgress);
			}
		}
	}
}
