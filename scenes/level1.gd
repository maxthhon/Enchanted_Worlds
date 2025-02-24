extends Node2D

var slime_preload = preload("res://objects/slime.tscn")

var life_time = 94
var start_time = 90

var game_flag = true
var game_start = true

func slime_spawn():
	var slime = slime_preload.instantiate()
	slime.position = Vector2(700 ,randi_range(100,400))
	$Spawner.add_child(slime)
	
func _on_timer_timeout():
	
	$Label2.text = " "
	life_time -= 1
	
	if game_start:
		$Label2.text = str(life_time - start_time) + "!"
		if life_time == start_time:
			game_start = false
		
	if life_time < 6 and life_time > -1:
		$Label2.text = str(life_time)
		$Label.text = " "
	elif life_time > -1:
		$Label.text = "Время игры: " + str(life_time)
	if life_time == 0:
		$Label.text = "Игра окончена!"
		game_flag = false
	if not $tower.tower_is_alive:
		$Label.text = "Игра окончена!"
		game_flag = false
		
	if (game_flag == false) and (life_time < -3):
		get_tree().change_scene_to_file("res://scenes/score_board.tscn")
		
func _on_spawn_time_timeout():
	if game_flag and not game_start:
		var chance = randi_range(1, 10)
		if chance >= 5:
			slime_spawn()

func _on_audio_stream_player_finished():
	$AudioStreamPlayer.play()
