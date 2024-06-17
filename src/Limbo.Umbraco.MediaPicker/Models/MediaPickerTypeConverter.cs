using System;
using System.Diagnostics.CodeAnalysis;
using Limbo.Umbraco.MediaPicker.Json.Newtonsoft;
using Newtonsoft.Json;

namespace Limbo.Umbraco.MediaPicker.Models;

/// <summary>
/// Class describing a selected type converter.
/// </summary>
[JsonConverter(typeof(MediaPickerTypeConverterJsonConverter))]
public class MediaPickerTypeConverter {

    /// <summary>
    /// Gets or sets the alias of the CLR type of the type converter.
    /// </summary>
    public string? Type { get; set; }

    /// <summary>
    /// Initializes a new instance with the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The alias of the CLR type of the type converter.</param>
    [SetsRequiredMembers]
    public MediaPickerTypeConverter(string type) {
        Type = type;
    }

    /// <summary>
    /// Initializes a new instance based on type <typeparamref name="T"/>.
    /// </summary>
    /// <typeparam name="T">The type of the type converter.</typeparam>
    /// <returns>An instance of <see cref="MediaPickerTypeConverter"/>.</returns>
    public static MediaPickerTypeConverter Create<T>() {
        return Create(typeof(T));
    }

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="type"/>.
    /// </summary>
    /// <param name="type">The type of the type converter.</param>
    /// <returns>An instance of <see cref="MediaPickerTypeConverter"/>.</returns>
    public static MediaPickerTypeConverter Create(Type type) {
        return new MediaPickerTypeConverter(MediaPickerUtils.GetTypeAlias(type));
    }

}