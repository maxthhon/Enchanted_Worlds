using Godot;
using System;

public partial class CoinCounter : CanvasLayer
{
	private Label _label;

	public override void _Ready()
	{
		_label = GetNode<Label>("Label");
		UpdateCoinCount(0); // начальное значение
	}

	public void UpdateCoinCount(int count, int required = 0)
	{
		if (required > 0)
			_label.Text = $"{count}/{required}";
		else
			_label.Text = count.ToString();
	}
}
