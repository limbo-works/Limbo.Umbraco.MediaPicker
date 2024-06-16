using System;
using Limbo.Umbraco.MediaPicker.Models;
using Skybrud.Essentials.Strings.Extensions;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Limbo.Umbraco.MediaPicker.Converters;

/// <summary>
/// Default implementation for an item converter. Converts <see cref="MediaWithCrops"/> to <see cref="MediaWithCropsItem"/>, or more specifically <see cref="ImageWithCropsItem"/> for images.
/// </summary>
public class DefaultMediaPickerItemConverter : MediaPickerItemConverterBase {

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    public DefaultMediaPickerItemConverter() {
        Name = "Default Item Converter";
    }

    /// <summary>
    /// Returns a common type for each item.
    /// </summary>
    /// <param name="propertyType">The property type.</param>
    /// <param name="config">The media picker configuration.</param>
    /// <returns>An instance of <see cref="Type"/>.</returns>
    protected override Type GetItemType(IPublishedPropertyType propertyType, LimboMediaPickerConfiguration config) {

        string[] filter = config.Filter.ToStringArray();

        if (filter.Length == 1) {
            return filter[0] switch {
                Constants.Conventions.MediaTypes.Image => typeof(ImageWithCropsItem),
                Constants.Conventions.MediaTypes.File => typeof(MediaWithCropsItem),
                _ => typeof(MediaWithCropsItem)
            };
        }

        return typeof(MediaWithCropsItem);

    }

    /// <summary>
    /// Methods responsible for converting a single <see cref="MediaWithCrops"/> item. If <see langword="null"/> is returned, the item is ignored.
    /// </summary>
    /// <param name="owner">The property owner.</param>
    /// <param name="propertyType">The property type.</param>
    /// <param name="source">The source.</param>
    /// <param name="config">The media picker configuration.</param>
    /// <returns>An instance representing the converted item.</returns>
    protected override object? ConvertItem(IPublishedElement owner, IPublishedPropertyType propertyType, MediaWithCrops source, LimboMediaPickerConfiguration config) {
        return source.ContentType.Alias switch {
            null => (object?) null,
            Constants.Conventions.MediaTypes.Image => new ImageWithCropsItem(source, config),
            Constants.Conventions.MediaTypes.File => new MediaWithCropsItem(source),
            _ => new MediaWithCropsItem(source)
        };
    }

}