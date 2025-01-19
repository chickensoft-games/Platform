# 🧩 Platform

Platform-specific native extensions for Godot. This is distributed as a GDExtension that GDScript users can use, as well as a [nuget package] that C# scripting users can add to their projects.

---

<p align="center">
<img alt="Platform" src="icon.png" width="200">
</p>

---

## ✅ Features

Right now, this project simply provides a way to determine the actual scale factor of the display a Godot window is located on. Godot does not determine the actual scale factor and cannot provide enough information to determine it accurately, so we use various native API's to determine the true scale factor.

- ✅ Determine scale factor on macOS.
- ✅ Determine scale factor on Windows.

## 📦 GDExtension Installation

You can download the zip file from the releases. For a better experience, consider using [GodotEnv] to install and manage it as an addon in your project. In your addons.json file, place the following:

```json
{
  "$schema": "https://chickensoft.games/schemas/addons.schema.json",
  "addons": {
    "platform": {
      "source": "zip",
      "subfolder": "platform",
      "url": "https://github.com/chickensoft-games/Platform/releases/download/0.4.0/platform.zip"
    }
  }
}
```

Then, run `godotenv addons install`. You should see a directory named `platform` in your project's `addons` directory.

You can easily invoke the extension from GDScript:

```gdscript
func _ready() -> void:
  var displays = Displays.new()
  var scaleFactor = displays.GetDisplayScaleFactor(get_window())
  print("scale factor: ", scaleFactor)
```

## 📦 Nuget Package Installation

```sh
dotnet add package Chickensoft.Platform --prerelease
```

Then, use it like so:

```csharp
using Chickensoft.Platform;
var systemScale = Displays.Singleton.GetDisplayScaleFactor(window);
```

That's it!

## 🛠️ Building the GDExtension

.NET AOT compilation does not support cross-OS builds yet. You'll only be able to build a few variants on a single OS.

You can use the included `build.sh` script to create the assemblies for your platform.

```sh
./build.sh macos
./build.sh linux
./build.sh windows
```

## 🛠️ Building the Nuget Package

The nuget package project shares the same source code and can be built directly.

```sh
cd Chickensoft.Platform
dotnet build
```

## 🤗 Contributing

The GDExtension version of this uses the [godot-dotnet] C# GDExtension bindings that are still being actively developed. Since godot-dotnet isn't published yet, we've added a `nuget.config` file to the project which contains the nightly package feed.

Note that you can enumerate all versions of godot-dotnet available for use by running:

```sh
nuget list -Source godot-dotnet -AllVersions -Prerelease
```

It helps if you have read some of the relevant documentation:

- [Microsoft: Building Native Libraries with NativeAOT](https://github.com/dotnet/samples/blob/main/core/nativeaot/NativeLibrary/README.md)
- [godot-dotnet: Godot.Bindings](https://github.com/raulsntos/godot-dotnet/tree/master/src/Godot.Bindings)
- [Godot: GDExtension C++ Example](https://docs.godotengine.org/en/stable/tutorials/scripting/gdextension/gdextension_cpp_example.html)
- [Source generation for platform invokes (p/invoke)](https://learn.microsoft.com/en-us/dotnet/standard/native-interop/pinvoke-source-generation)

As of right now, the `warning IL2104: Assembly 'Godot.Bindings' produced trim warnings` shown during build is a known issue. It doesn't prevent anything from working, however.

There is a Godot sandbox project in `sandbox/Chickensoft.Platform.Sandbox` that you can run to test the library manually on the platforms you have built for. **It has a readme with instructions on how to get it running.**

## 🙋‍♀️ Open Questions

Help wanted! We have a few outstanding open questions that we are seeking feedback on:

- [godot-dotnet]: Can we have reloadable GDExtensions written in C#?
- C#: Can we create native iOS libraries (XCFrameworks)?
- C#: Can we create native android libraries? [It seems possible][native-android-libs].
- Linux display scaling: it should be possible to implement display scale factor detection on linux if Godot does not provide an accurate scale factor. This could be done by detecting whether the environment is X11 or Wayland, and then invoking methods in libX11.so or libwayland-client.so, respectively.

If you know the answer to any of these, please open an issue or reach out to us in Discord! We are eager to support more platforms and improve this project.

[godot-dotnet]: https://github.com/raulsntos/godot-dotnet
[native-android-libs]: https://github.com/jonathanpeppers/Android-NativeAOT/blob/main/DotNet/libdotnet.csproj
[GodotEnv]: https://github.com/chickensoft-games/GodotEnv
[nuget package]: https://www.nuget.org/packages/Chickensoft.Platform/
