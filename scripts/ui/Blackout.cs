using Godot;
using System;

public partial class Blackout : DirectionalLight2D
{
	private bool isSwitchedOn = true;
	private Tween tween;

	// Делегат и событие для callback
	public delegate void TransitionCompleteCallback(bool isSwitchedOn);
	public event TransitionCompleteCallback OnTransitionComplete;

	public override void _Ready()
	{
		Energy = 2;
	}

	public void SetBlackout(bool switchOn)
	{
		isSwitchedOn = switchOn;

		// Если старый Tween ещё работает — убить его
		if (tween != null && tween.IsRunning())
			tween.Kill();

		// Новый tween
		tween = CreateTween();

		// Целевое значение энергии
		float targetEnergy = isSwitchedOn ? 2.0f : 0.0f;

		// Настройка анимации
		tween.TweenProperty(this, "energy", targetEnergy, 0.5f)
			.SetTrans(Tween.TransitionType.Sine)
			.SetEase(Tween.EaseType.InOut);

		// Когда анимация завершится — вызвать событие
		tween.Finished += () =>
		{
			OnTransitionComplete?.Invoke(isSwitchedOn);
		};
	}

	public async void Blink(float blinkTime = 0.15f)
	{
		// Если старый Tween ещё работает — убить его
		if (tween != null && tween.IsRunning())
			tween.Kill();

		// Включить blackout
		SetBlackout(true);

		// Подождать заданное время
		await ToSignal(GetTree().CreateTimer(blinkTime), "timeout");

		// Выключить blackout
		SetBlackout(false);
	}
}
