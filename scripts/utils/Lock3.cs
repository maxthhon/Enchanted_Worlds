using Godot;
using System;

public partial class Lock3 : Lock
{
	public override void _on_area_2d_body_entered(Node body)
	{
		if (body is Character)
		{
			// Убедись, что GameManager.Instance и Profile НЕ null
			if (GameManager.Instance?.Profile != null)
			{
				if (GameManager.Instance.Profile.HasItem("BronzeKey"))
				{
					GameManager.Instance.Profile.RemoveItem("BronzeKey", 1);
					QueueFree();
					
				} else {
					GD.PrintErr("Нет объекта ключ3");
				}
				
				
			}
			else
			{
				GD.PrintErr("GameManager.Instance или Profile == null");
			}
		}
	}
}
