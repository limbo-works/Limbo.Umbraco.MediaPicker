using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Limbo.Umbraco.MediaPicker.Converters;
using Limbo.Umbraco.MediaPicker.Json;
using Limbo.Umbraco.MediaPicker.Models;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Collections.Extensions;
using Skybrud.Essentials.Json.Extensions;
using Skybrud.Essentials.Strings.Extensions;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Core.Routing;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Cms.Infrastructure.DeliveryApi;
using Umbraco.Extensions;

#pragma warning disable CS1591

namespace Limbo.Umbraco.MediaPicker.PropertyEditors.ValueConverters;

public class LimboMediaPickerWithCropsValueConverter : MediaPickerWithCropsValueConverter {

    private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;
    private readonly IServiceProvider _serviceProvider;
    private readonly IJsonSerializer _jsonSerializer;
    private readonly IPublishedUrlProvider _publishedUrlProvider;
    private readonly IPublishedValueFallback _publishedValueFallback;

    private readonly ImageWithCropsTypeConverterCollection _converterCollection;

    #region Constructors

    public LimboMediaPickerWithCropsValueConverter(IPublishedSnapshotAccessor publishedSnapshotAccessor,
        IServiceProvider serviceProvider,
        IJsonSerializer jsonSerializer,
        IPublishedUrlProvider publishedUrlProvider,
        IPublishedValueFallback publishedValueFallback,
        ImageWithCropsTypeConverterCollection converterCollection,
        IApiMediaWithCropsBuilder apiMediaWithCropsBuilder) : base(publishedSnapshotAccessor, publishedUrlProvider, publishedValueFallback, jsonSerializer, apiMediaWithCropsBuilder) {
        _publishedSnapshotAccessor = publishedSnapshotAccessor;
        _serviceProvider = serviceProvider;
        _jsonSerializer = jsonSerializer;
        _publishedUrlProvider = publishedUrlProvider;
        _publishedValueFallback = publishedValueFallback;
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
        return propertyType.EditorAlias == LimboMediaPickerWithCropsPropertyEditor.EditorAlias;
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
        LimboMediaPickerWithCropsConfiguration? config = propertyType.DataType.ConfigurationAs<LimboMediaPickerWithCropsConfiguration>();
        if (config == null) throw new Exception("Can't continue without a configuration.");

        // Initialize a collection for the items
        List<object> items = new();

        // Get the UDIs from the intermediate value
        IEnumerable<MediaWithCropsDto> dtos = MediaWithCropsDeserializer.Deserialize(_jsonSerializer, inter);

        // Get the type converter (if any) and determine the item value type
        Type? modelType = TryGetConverter(config, out IImageWithCropsTypeConverter? converter) ? converter!.GetType(propertyType, config) : config.ValueType;

        // Attempt to get the current published snapshot
        if (_publishedSnapshotAccessor.TryGetPublishedSnapshot(out IPublishedSnapshot? publishedSnapshot)) {

            foreach (MediaWithCropsDto dto in dtos) {

                // Short-circuit on single item
                if (!config.Multiple && items.Count > 0) break;

                // Look up the media
                IPublishedContent? mediaItem = publishedSnapshot?.Media?.GetById(dto.MediaKey);

                if (mediaItem == null) continue;

                ImageCropperValue localCrops = new() {
                    Crops = dto.Crops,
                    FocalPoint = dto.FocalPoint,
                    Src = mediaItem.Url(_publishedUrlProvider)
                };

                localCrops.ApplyConfiguration(config);

                // TODO: This should be optimized/cached, as calling Activator.CreateInstance is slow
                var mediaWithCropsType = typeof(MediaWithCrops<>).MakeGenericType(mediaItem.GetType());
                var mediaWithCrops = (MediaWithCrops) Activator.CreateInstance(mediaWithCropsType, mediaItem, _publishedValueFallback, localCrops)!;

                // If the configuration doesn't specify a value type, we just create a new ImagePickerImage
                if (modelType == null) {
                    switch (mediaWithCrops.ContentType.Alias) {
                        case Constants.Conventions.MediaTypes.Image: items.Add(new ImageWithCropsItem(mediaWithCrops, config)); break;
                        case Constants.Conventions.MediaTypes.File: items.Add(new MediaWithCropsItem(mediaWithCrops)); break;
                        default: items.Add(new MediaWithCropsItem(mediaWithCrops)); break;
                    }
                    continue;
                }

                if (converter != null) {
                    items.Add(converter.Convert(owner, propertyType, mediaWithCrops, config));
                    continue;
                }

                // If the selected type has a constructor with an ImagePickerConfiguration as the second parameter, we choose that constructor
                if (HasConstructor<MediaWithCrops, LimboMediaPickerWithCropsConfiguration>(modelType)) {
                    items.Add(ActivatorUtilities.CreateInstance(_serviceProvider, modelType, mediaWithCrops, config));
                    continue;
                }

                items.Add(ActivatorUtilities.CreateInstance(_serviceProvider, modelType, mediaWithCrops));

            }

        }

        // Return the item(s) with the correct value type
        modelType ??= GetModelType(config);

        if (config.Multiple) {

            IEnumerable value = items;

            try {
                value = value.Cast(modelType);
            } catch (Exception ex) {
                throw new Exception($"Failed casting items to type {modelType}", ex);
            }

            try {
                value = value.ToList(modelType);
            } catch (Exception ex) {
                throw new Exception($"Failed converting items to list of type {modelType}", ex);
            }

            return value;

        }

        return config.Multiple ? items.Cast(modelType).ToList(modelType) : items.FirstOrDefault();

    }

    /// <summary>
    /// Returns the type of values returned by the converter.
    /// </summary>
    /// <param name="propertyType">The property type.</param>
    /// <returns>The CLR type of values returned by the converter.</returns>
    public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {

        // Call the base value converter if the config isn't the right type
        if (propertyType.DataType.Configuration is not LimboMediaPickerWithCropsConfiguration config) return base.GetPropertyValueType(propertyType);

        // Look up the selected converter and get it's desired type
        if (TryGetConverter(config, out IImageWithCropsTypeConverter? converter)) {
            Type type = converter!.GetType(propertyType, config);
            return config.Multiple ? typeof(IReadOnlyList<>).MakeGenericType(type) : type;
        }

        Type modelType = GetModelType(config);

        // If the data type allows multiple items, we should return IEnumerable<T> instead of T
        return config.Multiple ? typeof(IReadOnlyList<>).MakeGenericType(modelType) : modelType;

    }

    private Type GetModelType(LimboMediaPickerWithCropsConfiguration config) {

        // If the data type isn't configured with a type converter, we look for the value type option instead
        Type? modelType = config.ValueType;

        // If we still don't know the model type, we check whether the filter option is set to a single media type, and
        // if so, match the model type for the media type
        if (modelType == null) {
            string[] filter = config.Filter.ToStringArray();
            if (filter.Length == 1) {
                return filter[0] switch {
                    Constants.Conventions.MediaTypes.Image => typeof(ImageWithCropsItem),
                    Constants.Conventions.MediaTypes.File => typeof(MediaWithCropsItem),
                    _ => typeof(MediaWithCropsItem)
                };
            }
        }

        // And if we still don't know the model type, we fall back to "MediaWithCropsItem"
        return modelType ?? typeof(MediaWithCropsItem);

    }

    /// <summary>
    /// Returns whether the specified <paramref name="type"/> contains at least one constructor where the first
    /// parameter is of type <typeparamref name="T1"/> and the second parameter is of type <typeparamref name="T2"/>.
    ///
    /// Any additional parameter the constructors may have are not relevant here, as their values will be attempted
    /// to be solved using dependency injection.
    /// </summary>
    /// <typeparam name="T1">The type of the first parameter.</typeparam>
    /// <typeparam name="T2">The type of the second parameter.</typeparam>
    /// <param name="type">The type to check.</param>
    /// <returns><c>true</c> if at least one constructor is a match; otherwise <c>false</c>.</returns>
    private static bool HasConstructor<T1, T2>(Type type) {
        return type
            .GetConstructors(BindingFlags.Public | BindingFlags.Instance)
            .Select(cs => cs.GetParameters())
            .Any(ps => ps.Length >= 2 && ps[0].ParameterType == typeof(T1) && ps[1].ParameterType == typeof(T2));
    }

    private bool TryGetConverter(LimboMediaPickerWithCropsConfiguration config, out IImageWithCropsTypeConverter? converter) {
        converter = null;
        string? key = config.TypeConverter?.Type switch {
            JTokenType.Object => ((JObject) config.TypeConverter).GetString("key"),
            JTokenType.String => config.TypeConverter.ToString(),
            _ => null
        };
        return !string.IsNullOrWhiteSpace(key) && _converterCollection.TryGet(key, out converter);
    }

    #endregion

}