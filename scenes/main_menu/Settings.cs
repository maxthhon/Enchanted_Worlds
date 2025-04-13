using Godot;
using System;

public partial class Settings : Node2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		bool isHiDPIEnabled = (bool)ProjectSettings.GetSetting("display/window/dpi/allow_hidpi");
		
		if (isHiDPIEnabled)
		{
			GD.Print("HiDPI is enabled.");
		}
		else
		{
			GD.Print("HiDPI is disabled.");
		}
		var blackout = (Blackout)GetNode<DirectionalLight2D>("Blackout");
		blackout.OnTransitionComplete += HandleTransitionComplete;
		blackout.SetBlackout(false);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
	
	public void _on_button_pressed()
	{
		var blackout = (Blackout)GetNode<DirectionalLight2D>("Blackout");
		blackout.SetBlackout(true);
	}
	
	public void _on_button_2_pressed()
	{
		//
	}
	
	private void HandleTransitionComplete(bool isSwitchedOn)
	{
		if (isSwitchedOn)
		{
			GetTree().ChangeSceneToFile("res://scenes/main_menu/MainMenu.tscn");
		}
	}
	
}
