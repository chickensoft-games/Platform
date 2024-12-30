#!/bin/bash

# Remove the build directory
rm -rf ./build

if [ -z "$1" ]; then
  echo "Error: No platform specified. Please provide 'win', 'macOS', or 'linux'."
  exit 1
fi

# Execute commands based on the platform argument
case "$1" in
  "macos")
    echo "Publishing for macOS..."
    dotnet publish Chickensoft.Platform -v:detailed -o ./addons/platform/builds/macos --self-contained -c Debug -r osx-arm64
    dotnet publish Chickensoft.Platform -v:detailed -o ./addons/platform/builds/macos --self-contained -c Release -r osx-arm64
    dotnet publish Chickensoft.Platform -v:detailed -o ./addons/platform/builds/macos --self-contained -c Debug -r osx-x64
    dotnet publish Chickensoft.Platform -v:detailed -o ./addons/platform/builds/macos --self-contained -c Release -r osx-x64
    ;;

  "windows")
    echo "Publishing for Windows..."
    dotnet publish Chickensoft.Platform -v:detailed -o ./addons/platform/builds/windows --self-contained -c Debug -r win-x64
    dotnet publish Chickensoft.Platform -v:detailed -o ./addons/platform/builds/windows --self-contained -c Release -r win-x64
    ;;

  "linux")
    echo "Publishing for Linux..."
    dotnet publish Chickensoft.Platform -v:detailed -o ./addons/platform/builds/linux --self-contained -c Debug -r linux-x64
    dotnet publish Chickensoft.Platform -v:detailed -o ./addons/platform/builds/linux --self-contained -c Release -r linux-x64
    dotnet publish Chickensoft.Platform -v:detailed -o ./addons/platform/builds/linux --self-contained -c Debug -r linux-arm64
    dotnet publish Chickensoft.Platform -v:detailed -o ./addons/platform/builds/linux --self-contained -c Release -r linux-arm64
    ;;

  *)
    echo "Error: Invalid platform specified. Use 'win', 'macOS', or 'linux'."
    exit 1
    ;;
esac

echo "Build finished successfully."
exit 0
