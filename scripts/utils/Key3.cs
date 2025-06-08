using Godot;
using System;

public partial class Key3 : Key
{
	public override void _on_area_2d_body_entered(Node body)
	{
		if (body is Character)
		{
			// Убедись, что GameManager.Instance и Profile НЕ null
			if (GameManager.Instance?.Profile != null)
			{
				GameManager.Instance.Profile.AddItem("BronzeKey");
				QueueFree();
			}
			else
			{
				GD.PrintErr("GameManager.Instance или Profile == null");
			}
		}
	}
}
//гусь
