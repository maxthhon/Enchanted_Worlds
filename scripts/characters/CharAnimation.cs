using Godot;

public partial class CharAnimation : AnimatedSprite2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		Play();
		Animation = "stage";
	}
}
