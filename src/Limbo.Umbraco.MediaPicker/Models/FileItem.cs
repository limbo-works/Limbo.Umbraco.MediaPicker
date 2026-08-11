using System.Text.Json.Serialization;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Limbo.Umbraco.MediaPicker.Models;

/// <summary>
/// Class representing a file item.
/// </summary>
public class FileItem : MediaItem {

    #region Properties

    /// <summary>
    /// Gets the file size (bytes) of the file.
    /// </summary>
    [JsonPropertyName("bytes")]
    [JsonPropertyOrder(-450)]
    [Newtonsoft.Json.JsonProperty("bytes", Order = -450)]
    public virtual int Bytes { get; }
    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new file item.
    /// </summary>
    /// <param name="media">An instance of <see cref="IPublishedContent"/> representing the media.</param>
    public FileItem(IPublishedContent media) : base(media) {
        Bytes = media.Value<int>(Constants.Conventions.Media.Bytes);
    }

    #endregion

}