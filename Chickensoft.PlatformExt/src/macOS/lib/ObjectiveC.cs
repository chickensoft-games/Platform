namespace Chickensoft.Platform.MacOS.Lib;

using System;
using System.Runtime.InteropServices;

internal static partial class ObjectiveC {
  public const string LIB_OBJ_C = "/usr/lib/libobjc.dylib";
  public const int NULL = 0;

  [LibraryImport(LIB_OBJ_C, StringMarshalling = StringMarshalling.Utf8)]
  public static partial IntPtr sel_registerName(string selectorName);

  // Define methods for interacting with Cocoa API
  [LibraryImport(LIB_OBJ_C, StringMarshalling = StringMarshalling.Utf8)]
  public static partial IntPtr objc_getClass(string className);

  [LibraryImport(LIB_OBJ_C)]
  public static partial IntPtr objc_msgSend(IntPtr receiver, IntPtr selector);

  [LibraryImport(LIB_OBJ_C, StringMarshalling = StringMarshalling.Utf8)]
  public static partial IntPtr objc_msgSend(IntPtr receiver, IntPtr selector, string arg);

  [LibraryImport(LIB_OBJ_C)]
  public static partial IntPtr objc_msgSend(IntPtr receiver, IntPtr selector, IntPtr arg);

  [LibraryImport(LIB_OBJ_C)]
  public static partial IntPtr object_getClass(IntPtr obj);

  [LibraryImport(LIB_OBJ_C)]
  public static partial IntPtr class_getSuperclass(IntPtr obj);

  [LibraryImport(LIB_OBJ_C)]
  public static partial IntPtr class_getName(IntPtr cls);

  [LibraryImport(LIB_OBJ_C)]
  public static partial IntPtr class_respondsToSelector(IntPtr cls, IntPtr sel);
}
