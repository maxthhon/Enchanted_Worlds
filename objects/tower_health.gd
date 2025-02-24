extends StaticBody2D

class_name Tower

var health = 100
var max_health = 100
var tower_is_alive = true

func die():
	tower_is_alive = false
	$"../CharacterBody2D/Label2".text = "(отладка) ХАХАХАХХАХАХАХХАХАХХА"
	
func take_damage(amount):
	health -= amount
	g.tower_health = health
	$"../CharacterBody2D/Label2".text = "(отладка) damage!!!"
	$"../Progress_Bar/TextureProgressBar".set_value(health)
	if health <= 0:
		die()

func heal(amount):
	health = min(health + amount, max_health)
	g.tower_health = health
	$"../Progress_Bar/TextureProgressBar".set_value(health)

func _on_hitbox_body_entered(body):
	if body is Slime:
		take_damage(randi_range(5, 20))
		body.heal(randi_range(0, 2))
