// [CHANGE: Umbraco 13→17 upgrade] Related: all files under src/, see documentation/upgrade-to-umbraco-17.md
// Newtonsoft.Json attributes replaced with System.Text.Json equivalents.
using System.Text.Json.Serialization;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

// ReSharper disable UnusedParameter.Local

namespace Limbo.Umbraco.MediaPicker.Models;

/// <summary>
/// Class representing an image item.
/// </summary>
public class ImageItem : MediaItem {

    #region Properties

    /// <summary>
    /// The width of the media
    /// </summary>
    [JsonPropertyName("width")]
    [JsonPropertyOrder(-450)]
    public virtual int Width { get; }

    /// <summary>
    /// The height of the media
    /// </summary>
    [JsonPropertyName("height")]
    [JsonPropertyOrder(-400)]
    public virtual int Height { get; }

    /// <summary>
    /// The generated crop url
    /// </summary>
    [JsonPropertyName("cropUrl")]
    [JsonPropertyOrder(-300)]
    public virtual string CropUrl { get; }

    /// <summary>
    /// Gets the alternative text if an <c>altText</c> property exists on the media.
    /// </summary>
    [JsonPropertyName("altText")]
    [JsonPropertyOrder(-250)]
    public string AlternativeText => Media.Value<string>("altText") ?? string.Empty;
    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new image item.
    /// </summary>
    /// <param name="media">An instance of <see cref="IPublishedContent"/> representing the media.</param>
    /// <param name="config">The media picker data type configuration.</param>
    public ImageItem(IPublishedContent media, LimboMediaPickerConfiguration config) : base(media) {

        int width = media.Value<int>(Constants.Conventions.Media.Width);
        int height = media.Value<int>(Constants.Conventions.Media.Height);

        Width = width;
        Height = height;
        CropUrl = media.GetCropUrl(width, height, preferFocalPoint: config.PreferFocalPoint, imageCropMode: config.CropMode)!;

    }

    #endregion

}
