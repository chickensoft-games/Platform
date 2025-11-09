namespace Chickensoft.Platform;

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using Chickensoft.Platform.Utils;
using Godot;

/// <summary>
/// Linux-specific video output utilities.
/// </summary>
public static partial class VideoOutputs
{
  /// <summary>
  /// Represents a video output on linux. The video output data is inferred from
  /// xrandr and cross-referenced with /sys/class/drm to find native
  /// resolutions and compute the scale factor in use.
  /// </summary>
  /// <param name="Name">Name of the output, e.g., "DP-3"</param>
  /// <param name="LogicalResolution">Logical resolution of the output in
  /// pixels.</param>
  /// <param name="NativeResolution">Native physical pixel resolution.</param>
  /// <param name="PhysicalSizeMm">Physical size of the output in millimeters.
  /// </param>
  /// <param name="IsPreferred">Whether this output is marked as preferred in
  /// xrandr.</param>
  /// <param name="IsCurrent">Whether this output is currently active in
  /// xrandr.</param>
  public record VideoOutput(
    string Name,
    Vector2I LogicalResolution,
    Vector2I NativeResolution,
    Vector2I PhysicalSizeMm,
    bool IsPreferred,
    bool IsCurrent
  )
  {
    /// <summary>
    /// Calculates the physical DPI based on the native resolution and physical
    /// size.
    /// </summary>
    public int PhysicalDPI => CalculateDpi(NativeResolution, PhysicalSizeMm);

    /// <summary>
    /// Calculates the logical DPI based on the logical resolution and physical
    /// size. This should match closely what Godot provides.
    /// </summary>
    public int LogicalDpi => CalculateDpi(LogicalResolution, PhysicalSizeMm);

    private static int CalculateDpi(Vector2I resolution, Vector2I physicalMm)
    {
      if
      (
        physicalMm.X <= 0 || physicalMm.Y <= 0 ||
        resolution.X <= 0 || resolution.Y <= 0
      )
      {
        return 0;
      }

      var diagonalPixels = Math.Sqrt
      (
        ((double)resolution.X * resolution.X) +
        ((double)resolution.Y * resolution.Y)
      );

      var diagonalPhysicalMm = Math.Sqrt
      (
        ((double)physicalMm.X * physicalMm.X) +
        ((double)physicalMm.Y * physicalMm.Y)
      );

      var diagonalInches = diagonalPhysicalMm / 25.4; // convert mm to inches

      if (diagonalInches <= 0)
      {
        return 0;
      }

      // Godot floors as of 4.5.1, we round, so off by one is possible
      return (int)Math.Round(diagonalPixels / diagonalInches);
    }
  }

  // Group 1: flags (+ and/or *)
  // Group 2: output name
  // Group 3 logical width
  // Group 4: width in mm
  // Group 5 logical height
  // Group 6 height in mm
  // Group 7 output list (comma-separated) -- ignored in favor of output name
  [GeneratedRegex(@"^\s*\d+:\s+([+*]*)([A-Za-z0-9_.:-]+)\s+(\d+)\/(\d+)x(\d+)\/(\d+)\+\d+\+\d+\s+(.*)$")]
  private static partial Regex XRandRMonitorOutputRegex();

  /// <summary>
  /// Gets the video output most likely to be the one that matches the given
  /// logical resolution and DPI. This should generally allow us to resolve the
  /// monitor scale factor by querying xrandr for monitors which match the
  /// given logical resolution, and then cross-reference those with the native
  /// resolutions provided by /sys/class/drm to find the native resolution and
  /// compute the scale factor.
  /// </summary>
  /// <param name="logical">Logical resolution of the screen the Godot window
  /// is on.</param>
  /// <param name="dpi">Logical DPI of the screen the Godot window is on.
  /// </param>
  /// <returns>The most likely XRandR output that matches the given logical
  /// resolution and DPI, or null if no outputs could be found.</returns>
  public static VideoOutput? GetLikelyVideoOutput(Vector2I logical, int dpi)
  {
    var outputs = EnumerateVideoOutputs().ToList();

    outputs.Sort((a, b) =>
    {
      // Compute relative errors (percentage-based)
      var logicalErrorA = (double)((Vector2)
        ((a.LogicalResolution - logical) / logical)).LengthSquared();
      var logicalErrorB = (double)((Vector2)
        ((b.LogicalResolution - logical) / logical)).LengthSquared();

      var dpiErrorA = Math.Pow((a.LogicalDpi - dpi) / (double)dpi, 2);
      var dpiErrorB = Math.Pow((b.LogicalDpi - dpi) / (double)dpi, 2);

      var totalErrorA = logicalErrorA + dpiErrorA;
      var totalErrorB = logicalErrorB + dpiErrorB;

      return totalErrorA.CompareTo(totalErrorB);
    });

    return outputs.FirstOrDefault();
  }


  // Find the XRandR output that most closely matches the given logical
  // resolution and DPI hint that the Godot display server provides. match may
  // not be exact to either, so we can sort and do the math to find the one with
  // the least error for both.

  /// <summary>
  /// Enumerates all active XRandR outputs by invoking
  /// `xrandr --listactivemonitors`
  /// and parsing the output.
  /// </summary>
  /// <returns>An enumerable of video outputs.</returns>
  public static IEnumerable<VideoOutput> EnumerateVideoOutputs()
  {
    var result = Shell.Execute("xrandr", "--listactivemonitors");

    if (string.IsNullOrWhiteSpace(result.StandardOutput))
    {
      yield break;
    }

    var regex = XRandRMonitorOutputRegex();

    foreach (var raw in result.StandardOutput.Split('\n'))
    {
      var line = raw.TrimEnd();

      if (string.IsNullOrWhiteSpace(line))
      {
        continue;
      }

      if (line.StartsWith("Monitors:", StringComparison.Ordinal))
      {
        continue;
      }

      var match = regex.Match(line);

      if (!match.Success)
      {
        continue;
      }


      var flags = match.Groups[1].Value;
      var name = match.Groups[2].Value; // DP-3, eDP-1, HDMI-A-1
      var w = int.Parse(match.Groups[3].Value);
      var mmw = int.Parse(match.Groups[4].Value);
      var h = int.Parse(match.Groups[5].Value);
      var mmh = int.Parse(match.Groups[6].Value);

      var isPreferred = flags.Contains('+');
      var isCurrent = flags.Contains('*');

      var logical = new Vector2I(w, h);

      // fallback to logical resolution if we can't find native :(
      var native = GetNativeResolutionForXRandROutput(name) ?? logical;

      yield return new VideoOutput(
        Name: name,
        LogicalResolution: logical,
        NativeResolution: native,
        PhysicalSizeMm: new Vector2I(mmw, mmh),
        IsPreferred: isPreferred,
        IsCurrent: isCurrent
      );
    }

    yield break;
  }

  // xrOutput is something like "DP-3"
  private static Vector2I? GetNativeResolutionForXRandROutput(string xrOutput)
  {
    // everything is a file in linux
    foreach (var dir in Directory.GetDirectories("/sys/class/drm", "card*-*"))
    {
      var name = Path.GetFileName(dir); // e.g., card1-DP-3
      if (!name.EndsWith("-" + xrOutput, StringComparison.Ordinal))
      {
        continue;
      }

      var preferred = ReadFile(dir, "modes")
        .Split('\n').FirstOrDefault()?.Trim(); // string like "3840x2160"

      // extract width and height
      if (preferred is null)
      {
        continue;
      }

      var parts = preferred.Split('x');

      if (parts.Length != 2)
      {
        continue;
      }

      if (int.TryParse(parts[0], out var width) &&
          int.TryParse(parts[1], out var height))
      {
        return new Vector2I(width, height);
      }
    }

    return null;
  }

  private static string ReadFile(string dir, string file)
  {
    var path = Path.Combine(dir, file);
    return File.Exists(path) ? File.ReadAllText(path).Trim() : "";
  }
}
