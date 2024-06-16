using System.Diagnostics.CodeAnalysis;
using Limbo.Umbraco.MediaPicker.Json.Newtonsoft;
using Newtonsoft.Json;

namespace Limbo.Umbraco.MediaPicker.Models;

/// <summary>
/// Class describing a selected item converter.
/// </summary>
[JsonConverter(typeof(MediaPickerItemConverterJsonConverter))]
public class MediaPickerItemConverter {

    /// <summary>
    /// Gets or sets the alias of the item converter type.
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Initializes a new instance with the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The alias of the item converter type.</param>
    [SetsRequiredMembers]
    public MediaPickerItemConverter(string type) {
        Type = type;
    }

}