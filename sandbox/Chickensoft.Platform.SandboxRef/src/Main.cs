namespace Chickensoft.Platform.SandboxRef;

using Godot;

public partial class Main : Node2D
{

  public override void _Ready()
  {
    var displays = new Chickensoft.Platform.Displays();

    var window = GetWindow();
    var resolution = displays.GetNativeResolution(window);

    GD.Print($"Width: {resolution.X}, Height: {resolution.Y}");
  }
}
