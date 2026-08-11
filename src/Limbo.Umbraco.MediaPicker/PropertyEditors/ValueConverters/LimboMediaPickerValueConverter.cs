using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Limbo.Umbraco.MediaPicker.Converters;
using Limbo.Umbraco.MediaPicker.Models;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Infrastructure.DeliveryApi;

#pragma warning disable CS1591

namespace Limbo.Umbraco.MediaPicker.PropertyEditors.ValueConverters;

public class LimboMediaPickerValueConverter : MediaPickerWithCropsValueConverter {

    private readonly MediaPickerTypeConverterCollection _converterCollection;

    #region Constructors

    public LimboMediaPickerValueConverter(IPublishedMediaCache publishedMediaCache,
        IPublishedUrlProvider publishedUrlProvider,
        IPublishedValueFallback publishedValueFallback,
        IJsonSerializer jsonSerializer,
        IApiMediaWithCropsBuilder apiMediaWithCropsBuilder,
        MediaPickerTypeConverterCollection converterCollection) : base(publishedMediaCache, publishedUrlProvider, publishedValueFallback, jsonSerializer, apiMediaWithCropsBuilder) {
        _converterCollection = converterCollection;
    }

    #endregion

    #region Member methods

    /// <summary>
    /// Returns whether this class is the value converter for the specified <paramref name="propertyType"/>.
    /// </summary>
    /// <param name="propertyType">The property type.</param>
    /// <returns><c>true</c> if this class is the value converter for <paramref name="propertyType"/>; otherwise <c>false</c>.</returns>
    public override bool IsConverter(IPublishedPropertyType propertyType) {
        return propertyType.EditorAlias == LimboMediaPickerPropertyEditor.EditorAlias;
    }

    /// <summary>
    /// Converts the intermediate value to a corresponding object value.
    /// </summary>
    /// <param name="owner">The element holding the property type.</param>
    /// <param name="propertyType">The property type.</param>
    /// <param name="referenceCacheLevel">The reference cache level.</param>
    /// <param name="inter">The intermediate value.</param>
    /// <param name="preview">Whether preview mode is enabled.</param>
    /// <returns>The results of the conversion.</returns>
    public override object? ConvertIntermediateToObject(IPublishedElement owner, IPublishedPropertyType propertyType, PropertyCacheLevel referenceCacheLevel, object? inter, bool preview) {

        // Get the data type configuration
        LimboMediaPickerConfiguration? config = propertyType.DataType.ConfigurationAs<LimboMediaPickerConfiguration>();
        if (config == null) throw new Exception("Can't continue without a configuration.");

        object? value = base.ConvertIntermediateToObject(owner, propertyType, referenceCacheLevel, inter, preview);

        return TryGetConverter(config, out IMediaPickerTypeConverter? converter) ? converter.Convert(owner, propertyType, value, config) : value;

    }

    /// <summary>
    /// Returns the type of values returned by the converter.
    /// </summary>
    /// <param name="propertyType">The property type.</param>
    /// <returns>The CLR type of values returned by the converter.</returns>
    public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {

        // Call the base value converter if the config isn't the right type
        if (propertyType.DataType.ConfigurationAs<LimboMediaPickerConfiguration>() is not { } config) {
            return base.GetPropertyValueType(propertyType);
        }

        // Look up the selected converter and get it's desired type
        if (TryGetConverter(config, out IMediaPickerTypeConverter? converter)) {
            return converter.GetType(propertyType, config);
        }

        // Get the type of each item
        Type itemType = typeof(MediaWithCrops);

        // If the data type allows multiple items, we should return IReadOnlyList<T> instead of T
        return config.Multiple ? typeof(IReadOnlyList<>).MakeGenericType(itemType) : itemType;

    }

    private bool TryGetConverter(LimboMediaPickerConfiguration config, [NotNullWhen(true)] out IMediaPickerTypeConverter? converter) {
        converter = null;
        string? type = config.TypeConverter?.Type;
        return !string.IsNullOrWhiteSpace(type) && _converterCollection.TryGet(type, out converter);
    }

    #endregion

}