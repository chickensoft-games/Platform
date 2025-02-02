extends Control

func _ready() -> void:
	print("ready")
	var displays = Displays.new()
	var window := get_window()
	var scaleFactor = displays.GetDisplayScaleFactor(window)
	var resolution = displays.GetNativeResolution(window)
	print("scale factor: ", scaleFactor)
	print("native resolution: ", resolution)
