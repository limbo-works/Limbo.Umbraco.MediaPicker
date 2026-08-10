// [CHANGE: Umbraco 13→17 upgrade] Related: all files under src/, see documentation/upgrade-to-umbraco-17.md
// Newtonsoft.Json attributes replaced with System.Text.Json equivalents.
using System.Text.Json.Serialization;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Limbo.Umbraco.MediaPicker.Models;

/// <summary>
/// Class representing a media item.
/// </summary>
public class MediaItem {

    #region Properties

    /// <summary>
    /// Gets the IPublishedContent reference to the media node
    /// </summary>
    [JsonIgnore]
    [Newtonsoft.Json.JsonIgnore]
    public IPublishedContent Media { get; }

    /// <summary>
    /// The media int id
    /// </summary>
    [JsonPropertyName("id")]
    [JsonPropertyOrder(-500)]
    [Newtonsoft.Json.JsonProperty("id", Order = -500)]
    public virtual int Id => Media.Id;

    /// <summary>
    /// The url for the media
    /// </summary>
    [JsonPropertyName("url")]
    [JsonPropertyOrder(-350)]
    [Newtonsoft.Json.JsonProperty("url", Order = -350)]
    public virtual string Url => Media.Url();

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new image item.
    /// </summary>
    /// <param name="media">An instance of <see cref="IPublishedContent"/> representing the media.</param>.
    public MediaItem(IPublishedContent media) {
        Media = media;
    }

    #endregion

}
