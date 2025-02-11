extends Control

func _ready() -> void:
	print("ready")
	var displays = Displays.new()
	var window := get_window()
	var scaleFactor = displays.GetDisplayScaleFactor(window)
	print("scale factor: ", scaleFactor)
	var resolution = displays.GetNativeResolution(window)
	print("native resolution: ", resolution)
