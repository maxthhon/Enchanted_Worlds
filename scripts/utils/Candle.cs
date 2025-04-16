using Godot;
using System;

public partial class Candle : Node2D
{
	private int i = 0;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}
	
	static float CalculateY(int i)
	{
		return (float)(1.95 + 0.05 * Math.Sin((2 * Math.PI * i / 100) + (Math.PI / 2)));
	}
	
	static float CalculateYi(int i)
	{
		return (float)(0.95 + 0.05 * Math.Sin((2 * Math.PI * i / 100) + (Math.PI / 2)));
	}

	
	public void _on_timer_timeout()
	{
		if (i < 100) 
		{
			i += 1;
		} else {
			i = 0;
		}
		var fireLight = GetNode<Sprite2D>("fireLight");
		var Light = GetNode<PointLight2D>("LightArea");
		
		fireLight.Scale = new Vector2(1.0f, CalculateYi(i));
		Light.Scale = new Vector2(CalculateY(i), CalculateY(i));
	}
}
