using Godot;
using System;

public partial class Grass2 : StaticBody2D
{
	[Export]
	public int offset = 3;
	public override void _Ready()
	{
	}
	
	public void _on_tile_hit_box_area_entered(Area2D area)
	{
		//GD.Print(area);
		Position = new Vector2(Position.X, Position.Y + offset);
	}
	
	public void _on_tile_hit_box_area_exited(Area2D area)
	{
		//GD.Print(area);
		Position = new Vector2(Position.X, Position.Y - offset);
	}
	

}
