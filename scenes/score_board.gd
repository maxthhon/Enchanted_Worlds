extends Node2D

func _ready():
	var health = g.tower_health

	if health > 0:
		$Label.text = "YOU WON!"
		$Label2.text = "tower health: " + str(health)
		$Label3.text = "score: " + str(health * 10)
	else:
		$Label.text = "GAME OVER!"

func _on_button_pressed():
	get_tree().change_scene_to_file("res://scenes/level_manager.tscn")
