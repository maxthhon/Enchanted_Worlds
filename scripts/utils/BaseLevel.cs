using Godot;
using System;

public partial class BaseLevel : Node2D
{
	protected AudioStreamPlayer audioPlayer;
	protected Blackout blackout;

	public override void _Ready()
	{
		audioPlayer = GetNodeOrNull<AudioStreamPlayer>("AudioStreamPlayer");
		blackout = GetNodeOrNull<Blackout>("Blackout");

		if (blackout != null)
		{
			blackout.SetBlackout(false);
		}

		if (audioPlayer != null)
		{
			audioPlayer.Finished += OnAudioFinished;
			audioPlayer.Play();
		}
	}

	protected virtual void OnAudioFinished()
	{
		audioPlayer?.Play(); // Цикличное воспроизведение
	}

	public virtual void PauseGame()
	{
		GetTree().Paused = true;
	}

	public virtual void ResumeGame()
	{
		GetTree().Paused = false;
	}

	public virtual void TransitionToLevel(string path)
	{
		if (blackout != null)
		{
			blackout.OnTransitionComplete += (isOn) =>
			{
				if (isOn)
					GetTree().ChangeSceneToFile(path);
			};

			blackout.SetBlackout(true);
		}
		else
		{
			GetTree().ChangeSceneToFile(path);
		}
	}
}
