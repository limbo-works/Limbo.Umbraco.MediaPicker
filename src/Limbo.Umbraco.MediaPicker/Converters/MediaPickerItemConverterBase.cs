using System;
using System.Collections.Generic;
using Limbo.Umbraco.MediaPicker.Models;
using Skybrud.Essentials.Collections;
using Skybrud.Essentials.Collections.Enumerables.Extensions;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;

namespace Limbo.Umbraco.MediaPicker.Converters;

/// <summary>
/// Abstract class representing a base implementation of the <see cref="IMediaPickerTypeConverter"/>. Use this converter if
/// you wish to control how each item is converted.
/// </summary>
public abstract class MediaPickerItemConverterBase : IMediaPickerTypeConverter {

    #region Properties

    /// <summary>
    /// Gets the friendly name of the converter.
    /// </summary>
    public string Name { get; protected set; }

    /// <summary>
    /// Gets the icon of the item converter.
    /// </summary>
    public string? Icon { get; protected set; }

    #endregion

    #region Constructors

    /// <summary>
    /// Initializes a new instance.
    /// </summary>
    protected MediaPickerItemConverterBase() {
        Name = GetType().Name;
    }

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="name"/>.
    /// </summary>
    /// <param name="name">The friendly name of the converter.</param>
    protected MediaPickerItemConverterBase(string name) {
        Name = name;
    }

    /// <summary>
    /// Initializes a new instance based on the specified <paramref name="name"/> and <paramref name="icon"/>.
    /// </summary>
    /// <param name="name">The friendly name of the converter.</param>
    /// <param name="icon">The icon of the converter.</param>
    protected MediaPickerItemConverterBase(string name, string? icon) {
        Name = name;
        Icon = icon;
    }

    #endregion

    #region Member methods

    /// <summary>
    /// Returns an instance of <see cref="Type"/> representing the type for the overall property value.
    /// </summary>
    /// <param name="propertyType">The property type.</param>
    /// <param name="config">The media picker configuration.</param>
    /// <returns>An instance of <see cref="Type"/>.</returns>
    public Type GetType(IPublishedPropertyType propertyType, LimboMediaPickerConfiguration config) {
        Type itemType = GetItemType(propertyType, config);
        return config.Multiple ? typeof(IReadOnlyList<>).MakeGenericType(itemType) : itemType;
    }

    /// <summary>
    /// Returns a common type for each item.
    /// </summary>
    /// <param name="propertyType">The property type.</param>
    /// <param name="config">The media picker configuration.</param>
    /// <returns>An instance of <see cref="Type"/>.</returns>
    protected abstract Type GetItemType(IPublishedPropertyType propertyType, LimboMediaPickerConfiguration config);

    /// <summary>
    /// Returns the converted value based on <paramref name="source"/>.
    /// </summary>
    /// <param name="owner">The property owner.</param>
    /// <param name="propertyType">The property type.</param>
    /// <param name="source">The source.</param>
    /// <param name="config">The media picker configuration.</param>
    /// <returns>The converted value.</returns>
    public object? Convert(IPublishedElement owner, IPublishedPropertyType propertyType, object? source, LimboMediaPickerConfiguration config) {
        return source switch {
            null => config.Multiple ? ArrayUtils.Empty(GetItemType(propertyType, config)) : null,
            MediaWithCrops media => ConvertItem(owner, propertyType, media, config),
            IEnumerable<MediaWithCrops> media => ConvertList(owner, propertyType, media, config),
            _ => null
        };

    }

    /// <summary>
    /// Methods responsible for converting a single <see cref="MediaWithCrops"/> item. If <see langword="null"/> is returned, the item is ignored.
    /// </summary>
    /// <param name="owner">The property owner.</param>
    /// <param name="propertyType">The property type.</param>
    /// <param name="source">The source.</param>
    /// <param name="config">The media picker configuration.</param>
    /// <returns>An instance representing the converted item.</returns>
    protected abstract object? ConvertItem(IPublishedElement owner, IPublishedPropertyType propertyType, MediaWithCrops source, LimboMediaPickerConfiguration config);

    /// <summary>
    /// Method responsible for converting a collection of <see cref="MediaWithCrops"/> to a desired value.
    /// </summary>
    /// <param name="owner">The property owner.</param>
    /// <param name="propertyType">The property type.</param>
    /// <param name="source">The source.</param>
    /// <param name="config">The media picker configuration.</param>
    /// <returns>A list of converted items.</returns>
    protected object ConvertList(IPublishedElement owner, IPublishedPropertyType propertyType, IEnumerable<MediaWithCrops> source, LimboMediaPickerConfiguration config) {

        List<object> temp = [];

        foreach (MediaWithCrops media in source) {
            if (ConvertItem(owner, propertyType, media, config) is { } converted) temp.Add(converted);
        }

        Type itemType = GetItemType(propertyType, config);

        return temp.Cast(itemType).ToList(itemType);

    }

    #endregion

}