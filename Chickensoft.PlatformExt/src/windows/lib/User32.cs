namespace Chickensoft.Platform.Windows;

using System;
using System.Runtime.InteropServices;

internal static partial class User32 {
  public const int MONITOR_DEFAULTTONULL = 0x00000000;
  public const int MONITOR_DEFAULTTOPRIMARY = 0x00000001;
  public const int MONITOR_DEFAULTTONEAREST = 0x00000002;

#pragma warning disable IDE1006 // Naming Styles
  [StructLayout(LayoutKind.Sequential)]
  public struct MonitorInfo {

    public int cbSize;

    public Rect rcMonitor;
    public Rect rcWork;
    public uint dwFlags;
  }
#pragma warning restore IDE1006 // Naming Styles

  [StructLayout(LayoutKind.Sequential)]
  public struct Rect {
    public int Left;
    public int Top;
    public int Right;
    public int Bottom;

    public readonly int Width => Right - Left;
    public readonly int Height => Bottom - Top;
  }

  [LibraryImport("user32.dll", SetLastError = true)]
  public static partial IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

  [LibraryImport("user32.dll", SetLastError = true)]
  [return: MarshalAs(UnmanagedType.I1)]
  public static partial bool GetMonitorInfoA(
     IntPtr hMonitor,
    out MonitorInfo lpmi
  );
}
