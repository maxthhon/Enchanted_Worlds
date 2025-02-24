extends CharacterBody2D
class_name Slime

@export var max_speed = randi_range(20, 90)
@export var acceleration = randi_range(15, 75)
@export var deceleration = 100

var health = randi_range(5, 7)
var max_health = 7
var slime_is_alive = true
var slime_run = true

# Ссылка на Tower
@onready var tower = get_parent().get_parent().get_node("tower")

func _physics_process(delta):
	# Получение позиции Tower
	var tower_position = tower.global_position
	
	# Вычисление направления движения к Tower
	var direction = (tower_position - global_position - Vector2(0, -30)).normalized()
	
	# Ускорение или замедление в зависимости от направления
	if direction != Vector2.ZERO:
		# Ускорение
		velocity += direction * acceleration * delta
		velocity = velocity.limit_length(max_speed)
	else:
		# Замедление
		velocity = velocity.move_toward(Vector2.ZERO, deceleration * delta)
	if slime_run:
		# Движение врага
		move_and_slide()

func die():
	$end_life.start(0)
	$Slime_Ani.play("die")
	$Slime_Ani.flip_h = false
	
func take_damage(amount):
	if slime_is_alive:
		health -= amount
		$Slime_Ani.play("dam_take")
		if health <= 0:
			slime_is_alive = false
			die()

func heal(amount):
	health = min(health + amount, max_health)

func _on_visible_on_screen_notifier_2d_screen_exited():
	die()

func _on_timer_timeout():
	slime_run = not slime_run

func _on_end_life_timeout():
	g.acc = acceleration	
	queue_free()
