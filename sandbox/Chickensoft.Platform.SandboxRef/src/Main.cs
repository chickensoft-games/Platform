namespace Chickensoft.Platform.SandboxRef;

using Godot;

public partial class Main : Control
{
  public override void _Ready()
  {
    var window = GetWindow();

    var scaleFactor = Displays.Singleton.GetDisplayScaleFactor(window);
    var nativeResolution =
      Displays.Singleton.GetNativeResolution(window);

    var godotScreenSize =
      DisplayServer.Singleton.ScreenGetSize(window.CurrentScreen);
    var godotDpiScale =
      DisplayServer.Singleton.ScreenGetScale(window.CurrentScreen);

    GD.Print($"   Godot Screen Size: {godotScreenSize}");
    GD.Print($"     Godot DPI Scale: {godotDpiScale}");
    GD.Print($"Display Scale Factor: {scaleFactor}");
    GD.Print($"   Native Resolution: {nativeResolution}");

    QueueRedraw();
  }
}
