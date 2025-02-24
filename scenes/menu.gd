extends Node2D

@onready var animation_player = $AnimatedSprite2D
@onready var audio_player = $AudioStreamPlayer

func _ready():
	animation_player.play("stage")

func _on_button_2_pressed():
	get_tree().quit()
		
func _on_button_pressed():
	animation_player.play("go")
	$play_timer.start()

func _on_button_3_pressed():
	get_tree().change_scene_to_file("res://scenes/menu_about.tscn")

func _on_timer_timeout():
	animation_player.play("stage")
	get_tree().change_scene_to_file("res://scenes/level_manager.tscn")

func _on_audio_stream_player_finished():
	$AudioStreamPlayer.play()
