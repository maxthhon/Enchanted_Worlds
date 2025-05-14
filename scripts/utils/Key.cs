using Godot;
using System;

public partial class Key : StaticBody2D
{
	public void _on_area_2d_body_entered(Node2D body)
	{
		QueueFree();
	}
	
}
