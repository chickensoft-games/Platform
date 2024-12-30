extends Control

func _ready() -> void:
	var displays = Displays.new()
	var scaleFactor = displays.GetDisplayScaleFactor(get_window())
	print("scale factor: ", scaleFactor)
