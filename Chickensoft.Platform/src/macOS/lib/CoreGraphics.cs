namespace Chickensoft.Platform.MacOS.Lib;

using System;
using System.Runtime.InteropServices;

internal sealed partial class CoreGraphics {
#pragma warning disable IDE1006 // Naming Styles
  public const string LIB_CORE_GRAPHICS =
    "/System/Library/Frameworks/CoreGraphics.framework/CoreGraphics";
  public const uint kDisplayModeNativeFlag = 0x02000000;
#pragma warning restore IDE1006 // Naming Styles

  // Define CoreGraphics methods
  [LibraryImport(LIB_CORE_GRAPHICS)]
  public static partial IntPtr CGMainDisplayID();

  [LibraryImport(LIB_CORE_GRAPHICS)]
  public static partial IntPtr CGDisplayCopyAllDisplayModes(uint display, IntPtr options);

  [LibraryImport(LIB_CORE_GRAPHICS)]
  public static partial IntPtr CFArrayGetValueAtIndex(IntPtr array, int index);

  [LibraryImport(LIB_CORE_GRAPHICS)]
  public static partial int CFArrayGetCount(IntPtr array);

  [LibraryImport(LIB_CORE_GRAPHICS)]
  public static partial uint CGDisplayModeGetIOFlags(IntPtr mode);

  [LibraryImport(LIB_CORE_GRAPHICS)]
  public static partial int CGDisplayModeGetPixelWidth(IntPtr mode);

  [LibraryImport(LIB_CORE_GRAPHICS)]
  public static partial int CGDisplayModeGetPixelHeight(IntPtr mode);

  [LibraryImport(LIB_CORE_GRAPHICS)]
  public static partial int CGGetOnlineDisplayList(int maxDisplays, IntPtr onlineDisplays, ref int onlineDisplayCount);
}
