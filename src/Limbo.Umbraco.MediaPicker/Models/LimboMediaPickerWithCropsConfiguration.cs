using System;
using Newtonsoft.Json.Linq;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.MediaPicker.Models;

/// <summary>
/// Extends MediaPicker3 with our own additional fields.
/// </summary>
/// <seealso cref="MediaPicker3Configuration" />
public class LimboMediaPickerWithCropsConfiguration : MediaPicker3Configuration {

    private Type? _valueType;

    #region Properties

    /// <summary>
    /// Gets a reference to a <see cref="JToken"/> with information about the selected type converter.
    /// </summary>
    [ConfigurationField("typeConverter",
        "Type converter",
        $"/App_Plugins/{MediaPickerPackage.Alias}/Views/TypePicker.html?type=TypeConverter&editor=v3&v={{version}}",
        Description = "Select a type converter, which will be used for converting the selected items.")]
    public JToken? TypeConverter { get; set; }

    /// <summary>
    /// Gets the name of the value type. This will be used for resolving the <see cref="ValueType"/> parameter.
    /// </summary>
    [ConfigurationField("valueType",
        "Value type",
        $"/App_Plugins/{MediaPickerPackage.Alias}/Views/TypePicker.html?type=ValueType&editor=v3&v={{version}}",
        Description = "Select the .NET value type that should be used for representing the selected image(s).")]
    public string? ValueTypeName { get; set; }

    /// <summary>
    /// Gets the value type.
    /// </summary>
    public Type? ValueType => _valueType == null && string.IsNullOrWhiteSpace(ValueTypeName) == false ? _valueType = Type.GetType(ValueTypeName) : _valueType;

    /// <summary>
    /// Gets the crop mode to be used for the returned values. This property currently always returns <see cref="ImageCropMode"/>.
    /// </summary>
    public ImageCropMode CropMode => ImageCropMode.Crop;

    /// <summary>
    /// Gets whether generated URLs should prefer a focal point. This property currently always returns <c>true</c>.
    /// </summary>
    public bool PreferFocalPoint => true;

    #endregion

}