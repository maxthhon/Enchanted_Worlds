extends Area2D

@onready var slime = $".."
@onready var tower = get_parent().get_parent().get_node("tower")

func _on_body_entered(body):
	if body is Tower:
		slime.max_speed = 100
		slime.acceleration = randi_range(-70, -15)
		$"../Slime_Ani".flip_h = true
	if body is Player:
		if g.acc < 0:
			tower.heal(randi_range(5, 20))
