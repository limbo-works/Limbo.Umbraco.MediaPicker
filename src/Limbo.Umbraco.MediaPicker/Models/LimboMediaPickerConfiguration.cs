using MessagePack;
using Newtonsoft.Json.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.MediaPicker.Models;

/// <summary>
/// Extends MediaPicker3 with our own additional fields.
/// </summary>
/// <seealso cref="MediaPicker3Configuration" />
public class LimboMediaPickerConfiguration : MediaPicker3Configuration {

    #region Properties

    /// <summary>
    /// Gets a reference to a <see cref="JToken"/> with information about the selected item converter.
    /// </summary>
    [ConfigurationField("itemConverter",
        "Item converter",
        $"/App_Plugins/{MediaPickerPackage.Alias}/Views/ItemConverter.html?&v={{version}}",
        Description = "Select a item converter, which will be used for converting the selected items.")]
    public MediaPickerItemConverter? ItemConverter { get; set; }

    /// <summary>
    /// Gets the crop mode to be used for the returned values. This property currently always returns <see cref="ImageCropMode"/>.
    /// </summary>
    [IgnoreMember]
    public ImageCropMode CropMode => ImageCropMode.Crop;

    /// <summary>
    /// Gets whether generated URLs should prefer a focal point. This property currently always returns <c>true</c>.
    /// </summary>
    [IgnoreMember]
    public bool PreferFocalPoint => true;

    #endregion

}