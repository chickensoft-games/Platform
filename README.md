# 🧩 Platform

Platform-specific native extensions for Godot.

---

<p align="center">
<img alt="Platform" src="icon.png" width="200">
</p>

---

## ✅ Features

Platform provides a way to determine the actual scale factor and native resolution of the display that a Godot window is located on. Godot does not determine the actual scale factor and native resolution reliably on Windows and macOS, so we use various native API's to determine the true scale factor.

- ✅ Determine scale factor and native resolution on macOS.
- ✅ Determine scale factor and native resolution on Windows.

> [!NOTE]
> You may be more interested in using [GameTools], which uses this project to provide a high-level display-agnostic scaling API to help your game or app look consistent and scale correctly in multi-monitor mixed-DPI environments.

## 📦 Nuget Package Installation

```sh
dotnet add package Chickensoft.Platform --prerelease
```

Then, use it like so:

```csharp
using Chickensoft.Platform;

// Use Win32 or CoreGraphics to get the display's actual resolution.
var resolution = Displays.Singleton.GetNativeResolution(window);
// Use Win32 or CoreGraphics to determine the display's actual scale factor.
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

There is a Godot sandbox project in `sandbox/Chickensoft.Platform.SandboxRef` that you can run to test the library manually.

[GameTools]: https://github.com/chickensoft-games/GameTools
