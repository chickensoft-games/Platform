namespace Chickensoft.Platform.SandboxRef;

using Godot;

public partial class Main : Node2D {
  public const float THEME_SCALE = 4f;

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
    var monitorScale = displays.GetDisplayScaleFactor(window);
    var systemDpi = DisplayServer.ScreenGetDpi(window.CurrentScreen);
    var systemScale = systemDpi / 96.0f;
    var godotRes = DisplayServer.ScreenGetSize(window.CurrentScreen);
    var windowSize = window.Size;
    var contentScaleFactor = monitorScale / THEME_SCALE;
    var correctionFactor = monitorScale / systemScale;

    // The native resolution (true resolution of the monitor) and Godot's
    // understanding of the monitor resolution on Windows can be different,
    // since Godot does not have per-monitor DPI awareness on Windows (yet).
    // https://github.com/godotengine/godot/issues/56341
    GD.Print($"   Native Resolution: {resolution.X}, {resolution.Y}");
    GD.Print($"   Godot  Resolution: {godotRes.X}, {godotRes.Y}");
    GD.Print($"  True Monitor Scale: {monitorScale}");
    GD.Print($"        System Scale: {systemScale}");
    GD.Print($"              Window: {windowSize.X}, {windowSize.Y}");
    GD.Print($"Content Scale Factor: {contentScaleFactor}");
    GD.Print($"   Correction Factor: {correctionFactor}");
    GD.Print(
      $"      Project Window: {projectWindowSize.X}, {projectWindowSize.Y}"
    );
  }
}
