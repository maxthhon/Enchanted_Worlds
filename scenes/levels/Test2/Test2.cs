using Godot;
using System;

public partial class Test2 : Node2D
{
	public override void _Ready()
	{
		var blackout = (Blackout)GetNode<DirectionalLight2D>("Blackout");
		blackout.SetBlackout(false);
	}
}
