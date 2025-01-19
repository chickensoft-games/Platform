#!/bin/bash

# Remove the build directory
rm -rf ./build

if [ -z "$1" ]; then
  echo "Error: No platform specified. Please provide 'windows', 'macos', or 'linux'."
  exit 1
fi

# Execute commands based on the platform argument
case "$1" in
  "macos")
    echo "Publishing for macOS..."
    dotnet publish Chickensoft.PlatformExt -o ./addons/platform/builds/macos --self-contained -c Release -r osx-arm64
    dotnet publish Chickensoft.PlatformExt -o ./addons/platform/builds/macos --self-contained -c Release -r osx-x64
    ;;

  "windows")
    echo "Publishing for Windows..."
    dotnet publish Chickensoft.PlatformExt -o ./addons/platform/builds/windows --self-contained -c Release -r win-x64
    ;;

  "linux")
    echo "Publishing for Linux..."
    dotnet publish Chickensoft.PlatformExt -o ./addons/platform/builds/linux --self-contained -c Release -r linux-x64
    dotnet publish Chickensoft.PlatformExt -o ./addons/platform/builds/linux --self-contained -c Release -r linux-arm64
    ;;

  *)
    echo "Error: Invalid platform specified. Use 'windows', 'macos', or 'linux'."
    exit 1
    ;;
esac

echo "Build finished successfully."
exit 0
