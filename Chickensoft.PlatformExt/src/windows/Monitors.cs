namespace Chickensoft.Platform.Windows;

using System;
using Godot;

internal sealed class Monitors {
  /// <summary>
  /// Gets the Win32 monitor handle of the monitor displaying the Godot window.
  /// </summary>
  /// <param name="godotWindowId">Godot window id.</param>
  /// <returns>Win32 monitor handle.</returns>
  public static uint GetMonitorHandle(int godotWindowId) {
    var nativeHandle = DisplayServer
      .Singleton
      .WindowGetNativeHandle(DisplayServer.HandleType.WindowHandle, godotWindowId);

    var hWnd = new IntPtr(nativeHandle);
    return (uint)User32
      .MonitorFromWindow(hWnd, User32.MONITOR_DEFAULTTONEAREST)
      .ToInt32();
  }

  /// <summary>
  /// Determines the scale factor of the monitor. This should be the same as the
  /// Windows display setting scale factor that is configurable on the computer.
  /// </summary>
  /// <param name="hMonitor">Win32 monitor handle.</param>
  /// <returns>Monitor scale factor.</returns>
  public static float GetMonitorScale(uint hMonitor) {
    Shcore.GetDpiForMonitor(
      new IntPtr(hMonitor),
      Shcore.MONITOR_DPI_TYPE.MDT_EFFECTIVE_DPI,
      out var dpiX,
      out var dpiY
    );

    // https://stackoverflow.com/a/69573593
    return dpiY / 96f;
  }

  /// <summary>
  /// Determines the native pixel resolution of the monitor.
  /// </summary>
  /// <param name="hMonitor">Win32 monitor handle.</param>
  /// <returns>Native pixel resolution.</returns>
  public static unsafe Vector2I GetMonitorResolution(uint hMonitor) {
    var monitorInfo = new User32.MonitorInfo {
      cbSize = sizeof(User32.MonitorInfo)
    };

    if (!User32.GetMonitorInfoA(new IntPtr(hMonitor), out monitorInfo)) {
      return new Vector2I(0, 0);
    }

    return new Vector2I(
      monitorInfo.rcMonitor.Width,
      monitorInfo.rcMonitor.Height
    );
  }
}
