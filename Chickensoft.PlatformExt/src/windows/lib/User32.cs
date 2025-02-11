namespace Chickensoft.Platform.Windows;

using System;
using System.Runtime.InteropServices;

internal static partial class User32 {
  public const string USER32 = "user32.dll";
  public const int MONITOR_DEFAULTTONULL = 0x00000000;
  public const int MONITOR_DEFAULTTOPRIMARY = 0x00000001;
  public const int MONITOR_DEFAULTTONEAREST = 0x00000002;

#pragma warning disable IDE1006 // Naming Styles
  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Auto)]
  public sealed class MonitorInfoEx {
    public int cbSize;
    public Rect rcMonitor;
    public Rect rcWork;
    public int dwFlags;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
    public char[] szDevice;

    public MonitorInfoEx() {
      cbSize = Marshal.SizeOf<MonitorInfoEx>();
      szDevice = new char[32];
    }
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

  [LibraryImport(USER32, SetLastError = true)]
  public static partial IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);

  [LibraryImport(
    USER32,
    StringMarshalling = StringMarshalling.Utf16,
    SetLastError = true,
    EntryPoint = "GetMonitorInfoW"
  )]
  [return: MarshalAs(UnmanagedType.I1)]
  public static partial bool GetMonitorInfo(IntPtr hMonitor, IntPtr lpmi);

  public static readonly IntPtr DPI_AWARENESS_CONTEXT_UNAWARE = new(-1);
  public static readonly IntPtr DPI_AWARENESS_CONTEXT_SYSTEM_AWARE = new(-2);
  public static readonly IntPtr DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE =
    new(-3);
  public static readonly IntPtr DPI_AWARENESS_CONTEXT_PER_MONITOR_AWARE_V2 =
    new(-4);
  public static readonly IntPtr DPI_AWARENESS_CONTEXT_UNAWARE_GDISCALED =
    new(-5);

  [LibraryImport(USER32, SetLastError = true)]
  internal static partial IntPtr SetThreadDpiAwarenessContext(
    IntPtr dpiContext
  );
}
