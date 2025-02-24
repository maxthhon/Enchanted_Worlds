extends Node2D

func _on_button_pressed():
	get_tree().change_scene_to_file("res://scenes/menu.tscn")
	
func _on_button_2_pressed():
	get_tree().change_scene_to_file("res://scenes/level_1.tscn")
