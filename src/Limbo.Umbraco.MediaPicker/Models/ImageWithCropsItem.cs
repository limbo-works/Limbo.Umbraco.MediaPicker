// [CHANGE: Umbraco 13→17 upgrade] Related: all files under src/, see documentation/upgrade-to-umbraco-17.md
// Newtonsoft.Json attributes replaced with System.Text.Json equivalents.
using System.Text.Json.Serialization;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Extensions;

namespace Limbo.Umbraco.MediaPicker.Models;

/// <summary>
/// Class wrapping an instance <see cref="ImageWithCropsItem"/> where the underlying media is an image.
/// </summary>
public class ImageWithCropsItem : MediaWithCropsItem {

    #region Properties

    /// <summary>
    /// The width of the media
    /// </summary>
    [JsonPropertyName("width")]
    [JsonPropertyOrder(-450)]
    [Newtonsoft.Json.JsonProperty("width", Order = -450)]
    public virtual int Width { get; }

    /// <summary>
    /// The height of the media
    /// </summary>
    [JsonPropertyName("height")]
    [JsonPropertyOrder(-400)]
    [Newtonsoft.Json.JsonProperty("height", Order = -400)]
    public virtual int Height { get; }

    /// <summary>
    /// The generated crop url
    /// </summary>
    [JsonPropertyName("cropUrl")]
    [JsonPropertyOrder(-300)]
    [Newtonsoft.Json.JsonProperty("cropUrl", Order = -300)]
    public virtual string? CropUrl { get; }

    /// <summary>
    /// Gets the alt text if an "altText" property exists on the media
    /// </summary>
    [JsonPropertyName("altText")]
    [JsonPropertyOrder(-250)]
    [Newtonsoft.Json.JsonProperty("altText", Order = -250)]
    public string AlternativeText => Media.Value<string>("altText") ?? string.Empty;

    /// <summary>
    /// Gets a reference to the local crops for this media.
    /// </summary>
    [JsonPropertyName("localCrops")]
    [JsonPropertyOrder(-200)]
    [Newtonsoft.Json.JsonProperty("localCrops", Order = -200)]
    public ImageCropperValue LocalCrops => Media.LocalCrops;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a nw instance based on the specified <paramref name="media"/> and <paramref name="config"/>.
    /// </summary>
    /// <param name="media">The <see cref="MediaWithCrops"/> instance to wrap.</param>
    /// <param name="config">The media picker data type configuration.</param>
    public ImageWithCropsItem(MediaWithCrops media, LimboMediaPickerConfiguration config) : base(media) {

        int width = media.Value<int>(Constants.Conventions.Media.Width);
        int height = media.Value<int>(Constants.Conventions.Media.Height);

        Width = width;
        Height = height;
        CropUrl = media.GetCropUrl(width, height, preferFocalPoint: config.PreferFocalPoint, imageCropMode: config.CropMode);

    }

    #endregion

}
