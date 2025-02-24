extends CharacterBody2D
class_name Player
@export var max_speed = 75
@export var acceleration = 400
@export var deceleration = 600

var our_body
var damage_is_taked = false

func player_animations(direction: Vector2):
	var animation = get_node("Char_Ani")
	if not damage_is_taked:
		animation.flip_h = false
		if direction == Vector2(-1, 0):
			animation.set_animation("run_left")
		elif direction == Vector2(1, 0):
			animation.set_animation("run_right")
		elif direction == Vector2(0, -1):
			animation.set_animation("run_up")
		elif direction == Vector2(0, 1):
			animation.set_animation("run_down")
		elif direction == Vector2.ZERO:
			animation.set_animation("stage")
		else:
			animation.set_animation("run_down")
			
	elif direction == Vector2(-1, 0):
		animation.play("atack")
		animation.flip_h = true
	else:
		animation.play("atack")
		animation.flip_h = false
		
func get_input():
	var input_direction = Input.get_vector("left", "right", "up", "down")
	
	if our_body == null:
		pass
	else:
		if Input.is_action_pressed("space") and not damage_is_taked: 
			if our_body is Slime:
				damage_is_taked = true
				our_body.take_damage(randi_range(1, 3))
			$headdown.start(0)
	
	player_animations(input_direction)
	
	if input_direction != Vector2.ZERO:
		# Accelerate
		velocity += input_direction * acceleration * get_physics_process_delta_time()
		velocity = velocity.limit_length(max_speed)
	else:
		# Decelerate
		velocity = velocity.move_toward(Vector2.ZERO, deceleration * get_physics_process_delta_time())

func _physics_process(delta):
	get_input()
	move_and_slide()

	# Keep the character within the screen boundaries
	var screen_size = get_viewport().size
	position.x = clamp(position.x, 0, screen_size.x)
	position.y = clamp(position.y, 0, screen_size.y)

func _on_player_hit_box_body_entered(body):
	
	$"../CharacterBody2D/Label2".text = str(body)
	our_body = body
	
func _on_headdown_timeout():
	damage_is_taked = false
