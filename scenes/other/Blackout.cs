using Godot;
using System;

public partial class Blackout : DirectionalLight2D
{
	private bool isSwitchedOn = true;
	// Called when the node enters the scene tree for the first time.
	Timer timer;
	DirectionalLight2D blackout;

	// Делегат для callback-функции
	public delegate void TransitionCompleteCallback(bool isSwitchedOn);

	// Событие, которое будет вызывать callback-функцию
	public event TransitionCompleteCallback OnTransitionComplete;

	public override void _Ready()
	{
		timer = GetNode<Timer>("Timer");
		
		Energy = 2;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void _on_timer_timeout()
	{
		float targetEnergy = isSwitchedOn ? 2.0f : 0.0f;
		float expFactor = 0.1f; // Коэффициент экспоненциального приближения (0.0f до 1.0f)

		Energy = Energy * (1.0f - expFactor) + targetEnergy * expFactor;

		if (Mathf.Abs(Energy - targetEnergy) < 0.001f)
		{
			timer.Autostart = false;
			timer.Stop();

			// Вызов callback-функции
			OnTransitionComplete?.Invoke(isSwitchedOn);
		}
	}

	public void SetBlackout(bool isSwitchedOn)
	{
		this.isSwitchedOn = isSwitchedOn;
		timer.Autostart = true;
		timer.Start();
	}
}
