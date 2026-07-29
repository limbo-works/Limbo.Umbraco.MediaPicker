// [CHANGE: Umbraco 13→17 upgrade] Related: all files under src/, see documentation/upgrade-to-umbraco-17.md
// New typed response model replacing the Newtonsoft JObject previously returned by the API controller.
using System.Text.Json.Serialization;

namespace Limbo.Umbraco.MediaPicker.Models.Api;

/// <summary>
/// Response model describing a type converter as returned by the management API.
/// </summary>
public class MediaPickerTypeConverterModel {

    /// <summary>
    /// Gets the alias of the CLR type of the converter.
    /// </summary>
    [JsonPropertyName("type")]
    public required string Type { get; init; }

    /// <summary>
    /// Gets the friendly name of the converter.
    /// </summary>
    [JsonPropertyName("name")]
    public required string Name { get; init; }

    /// <summary>
    /// Gets the icon of the converter.
    /// </summary>
    [JsonPropertyName("icon")]
    public required string Icon { get; init; }

    /// <summary>
    /// Gets the full name of the assembly declaring the converter.
    /// </summary>
    [JsonPropertyName("assembly")]
    public string? Assembly { get; init; }

    /// <summary>
    /// Gets a description of the converter - eg. the name of the DLL declaring the converter.
    /// </summary>
    [JsonPropertyName("description")]
    public string? Description { get; init; }

    /// <summary>
    /// Gets information about the obsolete status of the converter, or <see langword="null"/> if the converter isn't obsolete.
    /// </summary>
    [JsonPropertyName("obsolete")]
    public MediaPickerTypeConverterObsoleteModel? Obsolete { get; init; }

}

/// <summary>
/// Response model describing the obsolete status of a type converter.
/// </summary>
public class MediaPickerTypeConverterObsoleteModel {

    /// <summary>
    /// Gets the obsolete message, if any.
    /// </summary>
    [JsonPropertyName("message")]
    public string? Message { get; init; }

}
