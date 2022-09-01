using Newtonsoft.Json;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Extensions;

// ReSharper disable UnusedParameter.Local

namespace Limbo.Umbraco.MediaPicker.Models {

    /// <summary>
    /// Class representing an image item.
    /// </summary>
    public class ImageItem : MediaItem {

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
        public virtual string CropUrl { get; }

        /// <summary>
        /// Gets the alternative text if an <c>altText</c> property exists on the media.
        /// </summary>
        [JsonProperty("altText", Order = -250)]
        public string AlternativeText => Media.Value<string>("altText") ?? string.Empty;
        #endregion

        #region Constructors

        /// <summary>
        /// Initializes a new image item.
        /// </summary>
        /// <param name="media">An instance of <see cref="IPublishedContent"/> representing the media.</param>.
        /// <param name="config">The ImagePicker config</param>
        public ImageItem(IPublishedContent media, LimboMediaPickerConfiguration config) : base(media) {

            int width = media.Value<int>(Constants.Conventions.Media.Width);
            int height = media.Value<int>(Constants.Conventions.Media.Height);

            Width = width;
            Height = height;
            CropUrl = media.GetCropUrl(width, height, preferFocalPoint: config.PreferFocalPoint, imageCropMode: config.CropMode)!;

        }

        #endregion

    }

}