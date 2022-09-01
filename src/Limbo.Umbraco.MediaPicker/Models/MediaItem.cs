using Newtonsoft.Json;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

namespace Limbo.Umbraco.MediaPicker.Models {

    /// <summary>
    /// Class representing a media item.
    /// </summary>
    public class MediaItem {

        #region Properties

        /// <summary>
        /// Gets the IPublishedContent reference to the media node
        /// </summary>
        [JsonIgnore]
        public IPublishedContent Media { get; }

        /// <summary>
        /// The media int id
        /// </summary>
        [JsonProperty("id", Order = -500)]
        public virtual int Id => Media.Id;

        /// <summary>
        /// The url for the media
        /// </summary>
        [JsonProperty("url", Order = -350)]
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

}