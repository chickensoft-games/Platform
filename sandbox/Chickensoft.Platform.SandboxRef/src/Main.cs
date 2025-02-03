namespace Chickensoft.Platform.SandboxRef;

using Godot;

public partial class Main : Node2D {
  public override void _Ready() {
    var displays = new Displays();

    var window = GetWindow();

    var screen = window.CurrentScreen;

    GD.Print($"Screen: {screen}");

    var projectWindowSize = new Vector2I(
      ProjectSettings.GetSetting("display/window/size/viewport_width").AsInt32(),
      ProjectSettings.GetSetting("display/window/size/viewport_height").AsInt32()
    );
    var resolution = displays.GetNativeResolution(window);
    var scaleFactor = displays.GetDisplayScaleFactor(window);
    var godotDpi = DisplayServer.ScreenGetDpi(window.CurrentScreen);
    var godotScaleFactor = godotDpi / 96.0f;
    var godotRes = DisplayServer.ScreenGetSize(window.CurrentScreen);
    var windowSize = window.Size;

    // The native resolution (true resolution of the monitor) and Godot's
    // understanding of the monitor resolution on Windows can be different,
    // since Godot does not have per-monitor DPI awareness on Windows (yet).
    // https://github.com/godotengine/godot/issues/56341
    GD.Print($"Native Resolution: {resolution.X}, {resolution.Y}");
    GD.Print($"Godot  Resolution: {godotRes.X}, {godotRes.Y}");
    GD.Print($"            Scale: {scaleFactor}");
    GD.Print($"        Godot DPI: {godotDpi}");
    GD.Print($"      Godot Scale: {godotScaleFactor}");
    GD.Print($"           Window: {windowSize.X}, {windowSize.Y}");
    GD.Print(
      $"   Project Window: {projectWindowSize.X}, {projectWindowSize.Y}"
    );
  }
}
