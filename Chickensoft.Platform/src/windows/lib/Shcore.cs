namespace Chickensoft.Platform.Windows;

using System;
using System.Runtime.InteropServices;


internal static partial class Shcore
{
  public const string SHCORE = "shcore.dll";
  public enum DEVICE_SCALE_FACTOR
  {
    DEVICE_SCALE_FACTOR_INVALID = 0,
    SCALE_100_PERCENT = 100,
    SCALE_120_PERCENT = 120,
    SCALE_125_PERCENT = 125,
    SCALE_140_PERCENT = 140,
    SCALE_150_PERCENT = 150,
    SCALE_160_PERCENT = 160,
    SCALE_175_PERCENT = 175,
    SCALE_180_PERCENT = 180,
    SCALE_200_PERCENT = 200,
    SCALE_225_PERCENT = 225,
    SCALE_250_PERCENT = 250,
    SCALE_300_PERCENT = 300,
    SCALE_350_PERCENT = 350,
    SCALE_400_PERCENT = 400,
    SCALE_450_PERCENT = 450,
    SCALE_500_PERCENT = 500
  };

  public enum MONITOR_DPI_TYPE
  {
    MDT_EFFECTIVE_DPI = 0,
    MDT_ANGULAR_DPI = 1,
    MDT_RAW_DPI = 2
  }

  [LibraryImport(SHCORE, SetLastError = true)]
  internal static partial int GetScaleFactorForMonitor(
    IntPtr hMonitor, out DEVICE_SCALE_FACTOR pScale
  );

  [LibraryImport(SHCORE, SetLastError = true)]
  internal static partial int GetDpiForMonitor(
    IntPtr hMonitor, MONITOR_DPI_TYPE dpiType, out uint dpiX, out uint dpiY
  );
}
