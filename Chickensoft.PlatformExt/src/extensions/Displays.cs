namespace Chickensoft.Platform;

using System.Runtime.InteropServices;
using Godot;

/// <summary>Platform-specific extensions for Godot.</summary>
#if GDEXTENSION
[GodotClass]
#endif
public sealed partial class Displays : RefCounted {
  /// <summary>
  /// Shared instance of the Displays class.
  /// </summary>
  public static Displays Singleton { get; } = new Displays();

  /// <summary>
  /// Computes the scale factor for the current desktop platform. On Windows,
  /// this invokes Win32 API calls to compute the actual scale factor of the
  /// screen that the window is on. On macOS, this computes the scale factor
  /// by using CoreGraphics to find the native pixel resolution of the screen
  /// and using it to form a ratio with the logical resolution of the screen
  /// (indicating the true scale factor for the system).
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

  /// <summary>
  /// Finds the native resolution of the screen that the given window is on using
  /// platform-specific API's on macOS and Windows.
  /// </summary>
  /// <param name="window">Godot window.</param>
  /// <returns>Native resolution on macOS or Windows.</returns>
#if GDEXTENSION
  [BindMethod]
#endif
  public Vector2I GetNativeResolution(Window window) {
    var id = window.GetWindowId();

    if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
      return MacOS.Displays.GetScreenResolution(
        MacOS.Displays.GetCGDirectDisplayID(id)
      );
    }

    else if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows)) {
      return Windows.Monitors.GetMonitorResolution(
        Windows.Monitors.GetMonitorHandle(id)
      );
    }

    return DisplayServer.Singleton.ScreenGetSize(window.CurrentScreen);
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

    var logicalResolutionRetina =
      DisplayServer.Singleton.ScreenGetSize(window.CurrentScreen);

    // Note that we divide by Godot's reported retina scale factor, since
    // the Godot DisplayServer has scaled the logical resolution by it.
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
