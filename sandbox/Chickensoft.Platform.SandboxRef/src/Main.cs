namespace Chickensoft.Platform.SandboxRef;

using Godot;

public partial class Main : Control {
  public const float THEME_SCALE = 4f;
  // When the resolution is the same as the theme design size, the theme should
  // be displayed without any scaling.
  public readonly Vector2I ThemeDesignSize = new(3840, 2160);

  public override void _Ready() {
    var displays = new Displays();

    var window = GetWindow();

    var screen = window.CurrentScreen;

    GD.Print($"Screen: {screen}");

    var projectWindowSize = new Vector2I(
      ProjectSettings.GetSetting("display/window/size/viewport_width").AsInt32(),
      ProjectSettings.GetSetting("display/window/size/viewport_height").AsInt32()
    );
    // Use Win32 or CoreGraphics to get the display's actual resolution.
    var resolution = displays.GetNativeResolution(window);
    // Use Win32 or CoreGraphics to determine the display's actual scale factor.
    var monitorScale = displays.GetDisplayScaleFactor(window);
    // Godot reports Windows' system scale factor, which may be different from
    // the monitor's scale factor since Godot does not opt-in to per-monitor DPI
    // awareness on Windows.
    var systemDpi = DisplayServer.ScreenGetDpi(window.CurrentScreen);
    // Windows scale factor can be determined by dividing by 96.
    var systemScale = systemDpi / 96.0f;
    // Get the size of the window from Godot, which is going to be in Godot's
    // system scale factor coordinate space.
    var godotRes = DisplayServer.ScreenGetSize(window.CurrentScreen);
    var windowSize = window.Size;
    // To convert from Godot to monitor scale
    var correctionFactor = monitorScale / systemScale;

    var themeScale = (float)resolution.Y / ThemeDesignSize.Y;

    // This content scale factor accounts for the actual monitor scaling and
    // godot's scaling so that the UI takes up roughly the same amount of
    // physical pixels regardless of the scaling chosen for the monitor. For
    // games, this is typically desired as the UI should be designed for a
    // sufficiently large resolution (like 4k) and then scaled to fit the
    // monitor's actual resolution. Users can offer scaling options in their
    // game and multiply this factor by the scaling option to get the final
    // scale, but this at least gives them a common frame of reference.
    var contentScaleFactor = themeScale / correctionFactor;

    var newWindowSize = new Vector2I(
      (int)(windowSize.X / correctionFactor),
      (int)(windowSize.Y / correctionFactor)
    );

    // The native resolution (true resolution of the monitor) and Godot's
    // understanding of the monitor resolution on Windows can be different,
    // since Godot does not have per-monitor DPI awareness on Windows (yet).
    // https://github.com/godotengine/godot/issues/56341
    GD.Print($"         Theme Scale: {themeScale}");
    GD.Print($"   Native Resolution: {resolution.X}, {resolution.Y}");
    GD.Print($"   Godot  Resolution: {godotRes.X}, {godotRes.Y}");
    GD.Print($"  True Monitor Scale: {monitorScale}");
    GD.Print($"        System Scale: {systemScale}");
    GD.Print($"Content Scale Factor: {contentScaleFactor}");
    GD.Print($"   Correction Factor: {correctionFactor}");
    GD.Print($"         Window Size: {windowSize.X}, {windowSize.Y}");
    GD.Print(
      $"      Project Window: {projectWindowSize.X}, {projectWindowSize.Y}"
    );
    GD.Print($"     New Window Size: {newWindowSize.X}, {newWindowSize.Y}");

    window.ContentScaleFactor = contentScaleFactor;
    window.Size = newWindowSize;
    QueueRedraw();
  }
}
