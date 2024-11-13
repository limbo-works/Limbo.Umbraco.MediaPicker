using MessagePack;
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
    /// Gets a reference to a <see cref="MediaPickerTypeConverter"/> with information about the selected type converter.
    /// </summary>
    [ConfigurationField("typeConverter",
        "Type converter",
        $"/App_Plugins/{MediaPickerPackage.Alias}/Views/TypeConverter.html?&v={{version}}",
        Description = "Select a type converter, which will be used for converting the default media picker value.")]
    public MediaPickerTypeConverter? TypeConverter { get; set; }

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