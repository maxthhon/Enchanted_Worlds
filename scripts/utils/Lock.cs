using Godot;
using System;

public partial class Lock : StaticBody2D
{
	public void _on_area_2d_body_entered(Node body)
	{
		if (body is Character)
		{
			// Убедись, что GameManager.Instance и Profile НЕ null
			if (GameManager.Instance?.Profile != null)
			{
				if (GameManager.Instance.Profile.HasItem("GoldKey"))
				{
					GameManager.Instance.Profile.RemoveItem("GoldKey", 1);
					QueueFree();
					
				} else {
					GD.PrintErr("Нет объекта ключ");
				}
				
				
			}
			else
			{
				GD.PrintErr("GameManager.Instance или Profile == null");
			}
		}
	}
}
