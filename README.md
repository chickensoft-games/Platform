# 🧩 Platform

Platform-specific native extensions for Godot.

---

<p align="center">
<img alt="Platform" src="icon.png" width="200">
</p>

---

## ✅ Features

Platform provides a way to determine the actual scale factor and native resolution of the display that a Godot window is located on. Godot does not determine the actual scale factor and native resolution on Windows, macOS, or Linux, so we use various native API's and tools to determine the native pixel resolution and actual (usable) scale factor for the game window.

- ✅ Determine scale factor and native resolution on Windows 10+.
- ✅ Determine scale factor and native resolution on macOS.
- ✅ Determine scale factor and native resolution on Linux.

> [!NOTE]
> You may be more interested in using [GameTools], which uses this project to provide a high-level display-agnostic scaling API to help your game or app look consistent and scale correctly in multi-monitor mixed-DPI environments.

Heuristic methods are used to determine display information based on the best available information from Godot in combination with native API calls (via P/Invoke) and system shell commands (on Linux). For the most part, these methods work well and should work correctly for the vast majority of users.

If you encounter an unexpected edge case that you feel can be solved, please open an issue and suggest how the correct behavior can be determined.

## 🪟 Windows

On Windows, Platform temporarily enables per-thread DPI awareness so that it can invoke the Win32 API's to correctly determine which monitor the window handled provided by Godot is on and its native pixel resolution. It also fetches the system scale factor since Windows' virtual desktop coordinates (device-independent pixels on Windows) do not make it easy to derive the scale factor.

## 🍎 macOS

On macOS, CoreGraphics is used to find the display for the native `NSWindow` handle provided by Godot. The display's modes are then searched to extract the native pixel resolution so that the correct scale factor can be derived.

## 🐧 Linux

On Linux, `xrandr` is used to find video outputs that match Godot's understanding of the logical resolution and DPI. The best matching display is then cross-referenced with `/sys/class/drm` to determine the actual native pixel resolution and derive the correct scale factor.

## 📦 Nuget Package Installation

Platform is available as a nuget package.

```sh
dotnet add package Chickensoft.Platform --prerelease
```

Then, use it like so:

```csharp
using Chickensoft.Platform;

// Get the display's actual resolution on Windows, macOS, or Linux.
var resolution = Displays.Singleton.GetNativeResolution(window);
// Get the display's actual scale factor on Windows, macOS, or Linux.
var scale = Displays.Singleton.GetDisplayScaleFactor(window);
```

That's it!

## 🛠️ Building the Nuget Package

The nuget package project shares the same source code and can be built directly.

```sh
cd Chickensoft.Platform
dotnet build
```

It helps if you have read some of the relevant documentation:

- [Microsoft: Building Native Libraries with NativeAOT](https://github.com/dotnet/samples/blob/main/core/nativeaot/NativeLibrary/README.md)
- [Source generation for platform invokes (p/invoke)](https://learn.microsoft.com/en-us/dotnet/standard/native-interop/pinvoke-source-generation)

There is a Godot sandbox project in `sandbox/Chickensoft.Platform.SandboxRef` that you can run to test the library manually from the operating system you are interested in testing.

[GameTools]: https://github.com/chickensoft-games/GameTools
