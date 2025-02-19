namespace Chickensoft.Platform.Windows;

using System;
using System.Diagnostics;
using System.Runtime.InteropServices;
using Godot;

internal sealed class Monitors {
  /// <summary>
  /// Gets the Win32 monitor handle of the monitor displaying the Godot window.
  /// </summary>
  /// <param name="godotWindowId">Godot window id.</param>
  /// <returns>Win32 monitor handle.</returns>
  public static long GetMonitorHandle(int godotWindowId) {
    var nativeHandle = DisplayServer
      .Singleton
      .WindowGetNativeHandle(
        DisplayServer.HandleType.WindowHandle, godotWindowId
      );

    var hWnd = new IntPtr(nativeHandle);
    return User32
      .MonitorFromWindow(hWnd, User32.MONITOR_DEFAULTTONEAREST).ToInt64();
  }

  /// <summary>
  /// Determines the scale factor of the monitor. This should be the same as the
  /// Windows display setting scale factor that is configurable on the computer.
  /// </summary>
  /// <param name="hMonitor">Win32 monitor handle.</param>
  /// <returns>Monitor scale factor.</returns>
  public static float GetMonitorScale(long hMonitor) {
    // We need to get the DPI of the monitor itself, not the system DPI.
    // Windows 10+ only.
    var oldDpiAwareness = User32.SetThreadDpiAwarenessContext(
      User32.DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2
    );

    Shcore.GetDpiForMonitor(
      new IntPtr(hMonitor),
      Shcore.MONITOR_DPI_TYPE.MDT_EFFECTIVE_DPI,
      out var dpiX,
      out var dpiY
    );

    // Restore previous thread dpi awareness context, just to be safe.
    User32.SetThreadDpiAwarenessContext(oldDpiAwareness);

    // https://stackoverflow.com/a/69573593
    return dpiY / 96f;
  }

  public static Vector2I GetMonitorResolution(long hMonitor) {
    var hMonitorPtr = new IntPtr(hMonitor);
    var monSize = Marshal.SizeOf<User32.MonitorInfoEx>();
    var pMonitorInfo = Marshal.AllocHGlobal(monSize);

    // We have to set the structure size first.
    Marshal.WriteInt32(pMonitorInfo, monSize);

    if (!User32.GetMonitorInfo(hMonitorPtr, pMonitorInfo)) {
      Debug.WriteLine(
        "Failed to get monitor info. Error Code: " +
          Marshal.GetLastWin32Error()
      );
      return Vector2I.Zero;
    }

    var monitorInfo =
      Marshal.PtrToStructure<User32.MonitorInfoEx>(pMonitorInfo);

    // Create a string from the szDevice char array. Why Windows wants to use
    // a string as a monitor identifier, I have no idea...

    var deviceChars = monitorInfo!.szDevice;
    var length = Array.IndexOf(deviceChars, '\0'); // stop at null terminator
    if (length < 0) {
      length = deviceChars.Length;
    }

    var deviceName = new string(deviceChars, 0, length);
    Debug.WriteLine($"Monitor Device Name: {deviceName}");

    // Create a device context for the monitor so we can use GDI api's.
    var hdc = Gdi32.CreateDC(null!, deviceName, null!, IntPtr.Zero);
    if (hdc == IntPtr.Zero) {
      Debug.WriteLine(
        "Failed to create device context. Error Code: " +
          Marshal.GetLastWin32Error()
      );
      return Vector2I.Zero;
    }

    try {
      // Actually get the monitor's native resolution :P
      var w = Gdi32.GetDeviceCaps(hdc, Gdi32.DESKTOP_HORZ_RES);
      var h = Gdi32.GetDeviceCaps(hdc, Gdi32.DESKTOP_VERT_RES);
      return new Vector2I(w, h).Abs();
    }
    finally {
      Gdi32.DeleteDC(hdc);
    }
  }
}
