using System.Text.Json.Serialization;

namespace Limbo.Umbraco.MediaPicker.Models.Api;

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