namespace Chickensoft.Platform.MacOS;

using Godot;
using ObjC = Lib.ObjectiveC;
using CG = Lib.CoreGraphics;

internal sealed partial class Displays : RefCounted {
  /// <summary>
  /// Given a pointer to an NSWindow instance, find the CGDirectDisplayID it is
  /// on. Godot provides a method to get the NSWindow pointer of the
  /// application via <see cref="DisplayServer.WindowGetNativeHandle(
  /// DisplayServer.HandleType, int)"/>.
  /// </summary>
  /// <param name="windowId">Godot window identifier. The CGDirectDisplayID of
  /// the display that the window is located on will be returned.</param>
  public static uint GetCGDirectDisplayID(int windowId = 0) {
    var nativeHandle = DisplayServer
      .Singleton
      .WindowGetNativeHandle(DisplayServer.HandleType.WindowHandle, windowId);

    var nsWindow = new IntPtr(nativeHandle);

    var selScreen = ObjC.sel_registerName("screen");
    var selDeviceDescription = ObjC.sel_registerName("deviceDescription");
    var selObjectForKey = ObjC.sel_registerName("objectForKey:");
    var strUtfStr = ObjC.sel_registerName("stringWithUTF8String:");

    // Get the NSScreen for the NSWindow
    var nsScreen = ObjC.objc_msgSend(nsWindow, selScreen);

    // Get the deviceDescription dictionary
    var deviceDescription = ObjC.objc_msgSend(nsScreen, selDeviceDescription);

    // Create an NSString for "NSScreenNumber"
    var nsScreenNumberKey = ObjC.objc_msgSend(ObjC.objc_getClass("NSString"), strUtfStr, "NSScreenNumber");

    // Get the value for the key "NSScreenNumber"
    var nsScreenNumber = ObjC.objc_msgSend(deviceDescription, selObjectForKey, nsScreenNumberKey);

    // Unbox NSNumber to UInt32 (assumes the number is an integer type)
    var selUnsignedIntValue = ObjC.sel_registerName("unsignedIntValue");
    var screenId = ObjC.objc_msgSend(nsScreenNumber, selUnsignedIntValue);

    return (uint)screenId.ToInt32();
  }

  /// <summary>
  /// Get the screen resolution of a display.
  /// </summary>
  /// <param name="displayId">Core graphics display id.</param>
  /// <returns>A vector containing the width and height in physical pixels,
  /// or <see cref="Vector2I.Zero"/> if the resolution could not be determined.
  /// </returns>
  public static Vector2I GetScreenResolution(uint displayId) {
    var modes = CG.CGDisplayCopyAllDisplayModes(displayId, ObjC.NULL);

    if (modes == ObjC.NULL) {
      return Vector2I.Zero;
    }

    // Find the native display mode for the display.
    var modeCount = CG.CFArrayGetCount(modes);
    for (var i = 0; i < modeCount; i++) {
      var mode = CG.CFArrayGetValueAtIndex(modes, i);

      if (
        (
          CG.CGDisplayModeGetIOFlags(mode) &
          CG.kDisplayModeNativeFlag
        ) != 0
      ) {
        // Native mode — extract physical pixel dimensions.
        var width = CG.CGDisplayModeGetPixelWidth(mode);
        var height = CG.CGDisplayModeGetPixelHeight(mode);

        return new(width, height);
      }
    }

    return Vector2I.Zero;
  }
}
