namespace Chickensoft.Platform.Windows;

using System;
using System.Runtime.InteropServices;

internal static partial class Gdi32 {
  public const string GDI32 = "gdi32.dll";
  public const int DESKTOP_VERT_RES = 117;
  public const int DESKTOP_HORZ_RES = 118;

  [LibraryImport(
    GDI32,
    SetLastError = true,
    StringMarshalling = StringMarshalling.Utf16,
    EntryPoint = "CreateDCW"
  )]
  internal static partial IntPtr CreateDC(
    string pwszDriver,   // typically null for display
    string pwszDevice,   // "\\.\DISPLAYx"
    string pwszOutput,   // null
    IntPtr lpInitData    // IntPtr.Zero
  );

  [LibraryImport(GDI32, SetLastError = true)]
  [return: MarshalAs(UnmanagedType.Bool)]
  internal static partial bool DeleteDC(IntPtr hdc);

  [LibraryImport(GDI32, SetLastError = true)]
  internal static partial int GetDeviceCaps(IntPtr hdc, int nIndex);
}
