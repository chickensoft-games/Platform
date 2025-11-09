namespace Chickensoft.Platform.Utils;

using System.Diagnostics;

internal static class Shell
{
  public sealed record ShellResult(
    int ExitCode,
    string? StandardOutput,
    string? StandardError
  );

  public static ShellResult Execute(string processPath, string arguments = "")
  {
    var process = new Process();

    process.StartInfo.FileName = processPath;
    process.StartInfo.Arguments = arguments;
    process.StartInfo.UseShellExecute = false;
    process.StartInfo.RedirectStandardOutput = true;
    process.StartInfo.RedirectStandardError = true;
    process.Start();

    var output = process.StandardOutput.ReadToEnd();
    var error = process.StandardError.ReadToEnd();

    process.WaitForExit();

    return new ShellResult(
      process.ExitCode,
      output,
      error
    );
  }
}
