// [CHANGE: Umbraco 13→17 upgrade] Related: all files under src/, see documentation/upgrade-to-umbraco-17.md
// SemVersion property removed - it was only used for cache busting the old AngularJS views.
using System;
using System.Diagnostics;

namespace Limbo.Umbraco.MediaPicker;

/// <summary>
/// Static class with various information and constants about the package.
/// </summary>
public static class MediaPickerPackage {

    /// <summary>
    /// Gets the alias of the package.
    /// </summary>
    public const string Alias = "Limbo.Umbraco.MediaPicker";

    /// <summary>
    /// Gets the friendly name of the package.
    /// </summary>
    public const string Name = "Limbo Media Picker";

    /// <summary>
    /// Gets the version of the package.
    /// </summary>
    public static readonly Version Version = typeof(MediaPickerPackage).Assembly.GetName().Version!;

    /// <summary>
    /// Gets the informational version of the package.
    /// </summary>
    public static readonly string InformationalVersion = FileVersionInfo
        .GetVersionInfo(typeof(MediaPickerPackage).Assembly.Location).ProductVersion!
        .Split('+')[0];

}
