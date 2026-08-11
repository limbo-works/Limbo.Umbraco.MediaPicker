using System;
using Limbo.Umbraco.MediaPicker.Models;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Limbo.Umbraco.MediaPicker.Converters;

/// <summary>
/// Interface describing a converter for converting instances of <see cref="MediaWithCrops"/> to another type.
/// </summary>
public interface IMediaPickerTypeConverter {

    /// <summary>
    /// Gets the alias of the converter.
    /// </summary>
    public sealed string Alias => MediaPickerUtils.GetTypeAlias(GetType());

    /// <summary>
    /// Gets the name of the converter.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Returns the CLR item type. As the <see cref="Convert"/> method may return different types, the <see cref="GetType"/> method should return a common type.
    /// </summary>
    /// <param name="propertyType">The property type.</param>
    /// <param name="config">The configuration of the parent data type.</param>
    /// <returns>An instance of <see cref="Type"/>.</returns>
    Type GetType(IPublishedPropertyType propertyType, LimboMediaPickerConfiguration config);

    /// <summary>
    /// Converts the <see cref="MediaWithCrops"/> <paramref name="source"/> value to the desired type.
    /// </summary>
    /// <param name="owner">The <see cref="IPublishedElement"/> holding the property with the image or media.</param>
    /// <param name="propertyType">The property type.</param>
    /// <param name="source">The <see cref="MediaWithCropsItem"/> value to be converted.</param>
    /// <param name="config">The configuration of the parent data type.</param>
    /// <returns>The desired output value based on the <see cref="MediaWithCrops"/>.</returns>
    object? Convert(IPublishedElement owner, IPublishedPropertyType propertyType, object? source, LimboMediaPickerConfiguration config);

}