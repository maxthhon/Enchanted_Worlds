using Godot;
using System;
using System.Collections.Generic;

public partial class MusicController : Node
{
	private AudioStreamPlayer _musicPlayer;

	private List<AudioStream> _tracks = new();
	private Dictionary<string, AudioStream> _trackMap = new(); // <название, трек>
	private int _currentTrackIndex = 0;

	private float _pausedPosition = 0f;
	private bool _isPaused = false;

	public override void _Ready()
	{
		_musicPlayer = GetNode<AudioStreamPlayer>("MusicPlayer");
		_musicPlayer.Finished += OnMusicFinished; // Подписка на метод

		GD.Print("MusicController READY");

		// Добавляем треки: название => путь
		AddTrack("ForestTheme_Begin", "res://assets/audio/music/ForestTheme_Begin.wav");
		AddTrack("ForestTheme_LittleBlueberry", "res://assets/audio/music/ForestTheme_LittleBlueberry.wav");
		AddTrack("ForestTheme_OurGlade", "res://assets/audio/music/ForestTheme_OurGlade.wav");

		AddTrack("LibraryTheme_Begin", "res://assets/audio/music/LibraryTheme_Begin.wav");
		AddTrack("LibraryTheme_End", "res://assets/audio/music/LibraryTheme_End.wav");
		AddTrack("LibraryTheme_NewAdventure", "res://assets/audio/music/LibraryTheme_NewAdventure.wav");
		AddTrack("LibraryTheme_NewBegin", "res://assets/audio/music/LibraryTheme_NewBegin.wav");
		AddTrack("LibraryTheme_Run", "res://assets/audio/music/LibraryTheme_Run.wav");

		PlayTrack(_currentTrackIndex);
		GD.Print("Треки загружены успешно");
	}

	private void OnMusicFinished()
	{
		// Перезапускаем тот же трек
		_musicPlayer.Play();
	}

	private void AddTrack(string name, string path)
	{
		var stream = ResourceLoader.Load<AudioStream>(path);
		if (stream != null)
		{
			_tracks.Add(stream);
			_trackMap[name] = stream;
		}
		else
		{
			GD.PrintErr($"Не удалось загрузить трек: {path}");
		}
	}

	public void ToggleMusic()
	{
		if (_musicPlayer.Playing && !_isPaused)
		{
			_pausedPosition = _musicPlayer.GetPlaybackPosition();
			_musicPlayer.Stop();
			_isPaused = true;
		}
		else if (_isPaused)
		{
			_musicPlayer.Play(_pausedPosition);
			_isPaused = false;
		}
		else
		{
			_musicPlayer.Play();
		}
	}

	public void StopMusic()
	{
		_musicPlayer.Stop();
		_pausedPosition = 0f;
		_isPaused = false;
	}

	public void SetVolume(float volume)
	{
		_musicPlayer.VolumeDb = LinearToDb(volume);
	}

	private float LinearToDb(float linear)
	{
		if (linear <= 0.0f)
			return -80.0f;
		return (float)(20.0 * Math.Log10(linear));
	}

	public void NextTrack()
	{
		_currentTrackIndex = (_currentTrackIndex + 1) % _tracks.Count;
		PlayTrack(_currentTrackIndex);
	}

	public void PreviousTrack()
	{
		_currentTrackIndex = (_currentTrackIndex - 1 + _tracks.Count) % _tracks.Count;
		PlayTrack(_currentTrackIndex);
	}

	private void PlayTrack(int index)
	{
		if (index < 0 || index >= _tracks.Count)
			return;

		_pausedPosition = 0f;
		_isPaused = false;

		_musicPlayer.Stream = _tracks[index];
		_musicPlayer.Play();
	}

	public void PlayTrackByName(string name)
	{
		if (_trackMap.TryGetValue(name, out var stream))
		{
			// Проверяем, играет ли уже этот трек
			if (_musicPlayer.Stream == stream && _musicPlayer.Playing && !_isPaused)
			{
				return; // Уже играет — не переключаем
			}

			_pausedPosition = 0f;
			_isPaused = false;

			_musicPlayer.Stream = stream;
			_musicPlayer.Play();
		}
		else
		{
			GD.PrintErr($"Трек с именем '{name}' не найден.");
		}
	}
}
