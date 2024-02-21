using Newtonsoft.Json;
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
    [JsonProperty("width", Order = -450)]
    public virtual int Width { get; }

    /// <summary>
    /// The height of the media
    /// </summary>
    [JsonProperty("height", Order = -400)]
    public virtual int Height { get; }

    /// <summary>
    /// The generated crop url
    /// </summary>
    [JsonProperty("cropUrl", Order = -300)]
    public virtual string? CropUrl { get; }

    /// <summary>
    /// Gets the alt text if an "altText" property exists on the media
    /// </summary>
    [JsonProperty("altText", Order = -250)]
    public string AlternativeText => Media.Value<string>("altText") ?? string.Empty;

    /// <summary>
    /// Gets a reference to the local crops for this media.
    /// </summary>
    [JsonProperty("localCrops", Order = -200)]
    public ImageCropperValue LocalCrops => Media.LocalCrops;

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a nw instance based on the specified <paramref name="media"/> and <paramref name="config"/>.
    /// </summary>
    /// <param name="media">The <see cref="MediaWithCrops"/> instance to wrap.</param>
    /// <param name="config">The ImagePicker config</param>
    public ImageWithCropsItem(MediaWithCrops media, LimboMediaPickerWithCropsConfiguration config) : base(media) {

        int width = media.Value<int>(Constants.Conventions.Media.Width);
        int height = media.Value<int>(Constants.Conventions.Media.Height);

        Width = width;
        Height = height;
        CropUrl = media.GetCropUrl(width, height, preferFocalPoint: config.PreferFocalPoint, imageCropMode: config.CropMode);

    }

    #endregion

}