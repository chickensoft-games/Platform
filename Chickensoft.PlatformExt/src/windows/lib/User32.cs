namespace Chickensoft.Platform.Windows;

using System;
using System.Runtime.InteropServices;

internal static partial class User32 {
  public const int MONITOR_DEFAULTTONULL = 0x00000000;
  public const int MONITOR_DEFAULTTOPRIMARY = 0x00000001;
  public const int MONITOR_DEFAULTTONEAREST = 0x00000002;

  [LibraryImport("user32.dll", SetLastError = true)]
  public static partial IntPtr MonitorFromWindow(IntPtr hwnd, uint dwFlags);
}
