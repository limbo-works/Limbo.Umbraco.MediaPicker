using Newtonsoft.Json;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

// ReSharper disable UnusedParameter.Local

namespace Limbo.Umbraco.MediaPicker.Models {

    /// <summary>
    /// Class representing a file item.
    /// </summary>
    public class FileItem : MediaItem {

        #region Properties

        /// <summary>
        /// Gets the file size (bytes) of the file.
        /// </summary>
        [JsonProperty("bytes", Order = -450)]
        public virtual int Bytes { get; }
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new file item.
        /// </summary>
        /// <param name="media">An instance of <see cref="IPublishedContent"/> representing the media.</param>.
        /// <param name="config">The ImagePicker config</param>
        public FileItem(IPublishedContent media, LimboMediaPickerConfiguration config) : base(media) {
            Bytes = media.Value<int>(Constants.Conventions.Media.Bytes);
        }

        #endregion

    }

}