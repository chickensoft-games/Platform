# Platform Sandbox

This is a Godot project you can run on the relevant platforms to manually verify whether or not the native platform extensions are working as expected.

This uses [GodotEnv](https://github.com/chickensoft-games/GodotEnv) to manage a symlink to the `root/addons` directory, so you'll need to have it installed and use it to "install" addons.

```sh
dotnet tool install -g Chickensoft.GodotEnv
```

Then, from this directory, run:

```sh
godotenv addons install
```
