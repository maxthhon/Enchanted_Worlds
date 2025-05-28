using Godot;
using System;

public partial class Exit : StaticBody2D
{
	public void _on_area_2d_body_entered(Node body)
	{
		if (body is Character)
		{
			GameManager.Instance.ExitLevel();
		}
	}
}
