namespace Chickensoft.Platform;

using System.Runtime.InteropServices;
using Godot;

/// <summary>Platform-specific extensions for Godot.</summary>
#if GDEXTENSION
[GodotClass]
#endif
public sealed partial class Displays : RefCounted {
  /// <summary>
  ///
  /// </summary>
  /// <param name="window">Godot window.</param>
  /// <returns>The scale factor of the window.</returns>
#if GDEXTENSION
  [BindMethod]
#endif
  public float GetDisplayScaleFactor(Window window) {
    var id = window.GetWindowId();

    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
      return GetDisplayScaleFactorMacOS(window);
    }

    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
      return GetDisplayScaleFactorWindows(window);
    }

    return DisplayServer.Singleton.ScreenGetScale(window.CurrentScreen);
  }

  private static float GetDisplayScaleFactorMacOS(Window window) {
    // This will always be 1, 2, or 3, due to limited information from macOS.
    // This scale factor represents the type of retina display, but not the
    // user-adjusted logical resolution scaling. We can determine the actual
    // scale factor by scaling this against the logical resolution divided by
    // the native resolution.
    var retinaScale =
      DisplayServer.Singleton.ScreenGetScale(window.CurrentScreen);

    var cgDisplayId = MacOS.Displays.GetCGDirectDisplayID(window.GetWindowId());
    var nativeResolution = MacOS.Displays.GetScreenResolution(cgDisplayId);

    var logicalResolutionRetina = DisplayServer.Singleton.ScreenGetSize();
    var logicalResolution = new Vector2I(
      Mathf.RoundToInt(logicalResolutionRetina.X / retinaScale),
      Mathf.RoundToInt(logicalResolutionRetina.Y / retinaScale)
    );

    var scaleFactor = (float)nativeResolution.Y / logicalResolution.Y;

    return scaleFactor;
  }

  private static float GetDisplayScaleFactorWindows(Window window) {
    var hMonitor = Windows.Monitors.GetMonitorHandle(window.GetWindowId());
    return Windows.Monitors.GetMonitorScale(hMonitor);
  }
}
