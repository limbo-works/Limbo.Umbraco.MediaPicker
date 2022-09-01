using System;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.MediaPicker.Models {

    /// <summary>
    /// Extends the mediapicker configuration and adds our own fields.
    /// </summary>
    /// <seealso cref="LimboMediaPickerConfiguration" />
    public class LimboMediaPickerConfiguration : MediaPickerConfiguration {

        private Type? _valueType;

        #region Properties

        /// <summary>
        /// Gets the name of the value type. This will be used for resolving the <see cref="ValueType"/> parameter.
        /// </summary>
        [ConfigurationField("valueType",
            "Value type",
            $"/App_Plugins/{MediaPickerPackage.Alias}/Views/TypePicker.html?type=ValueType&editor=v2&v={{version}}",
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

}
