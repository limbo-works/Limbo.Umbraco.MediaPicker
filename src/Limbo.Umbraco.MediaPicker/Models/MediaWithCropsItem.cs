using Newtonsoft.Json;
using Umbraco.Cms.Core.Models;
using Umbraco.Extensions;

namespace Limbo.Umbraco.MediaPicker.Models {

    /// <summary>
    /// Base class wrapping an instance of <see cref="MediaWithCrops"/>.
    /// </summary>
    public class MediaWithCropsItem {

        #region Properties

        /// <summary>
        /// Gets a reference to the underlying <see cref="MediaWithCrops"/>.
        /// </summary>
        [JsonIgnore]
        public MediaWithCrops Media { get; }

        /// <summary>
        /// Gets the numeric ID of the underlying media.
        /// </summary>
        [JsonProperty("id", Order = -500)]
        public virtual int Id => Media.Id;

        /// <summary>
        /// The URL of the underlying media.
        /// </summary>
        [JsonProperty("url", Order = -350)]
        public virtual string Url => Media.Url();

        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a nw instance based on the specified <paramref name="media"/>.
        /// </summary>
        /// <param name="media">The <see cref="MediaWithCrops"/> instance to wrap.</param>
        public MediaWithCropsItem(MediaWithCrops media) {
            Media = media;
        }

        #endregion

    }

}