using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Limbo.Umbraco.MediaPicker.Models;
using Microsoft.Extensions.DependencyInjection;
using Skybrud.Essentials.Collections.Extensions;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.PublishedCache;

namespace Limbo.Umbraco.MediaPicker.PropertyEditors.ValueConverters {

    /// <summary>
    /// Extends the mediapicker value converter and ensures we can return our own types.
    /// </summary>
    /// <seealso cref="MediaPickerValueConverter" />
    public class LimboMediaPickerValueConverter : MediaPickerValueConverter {

        private readonly IPublishedSnapshotAccessor _publishedSnapshotAccessor;
        private readonly IServiceProvider _serviceProvider;

        #region Constructors

        /// <summary>
        /// Initializes a new instance of the <see cref="LimboMediaPickerValueConverter"/> class.
        /// </summary>
        /// <param name="publishedSnapshotAccessor">The published snapshot accessor.</param>
        /// <param name="publishedModelFactory">The published model factory.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public LimboMediaPickerValueConverter(IPublishedSnapshotAccessor publishedSnapshotAccessor, IPublishedModelFactory publishedModelFactory, IServiceProvider serviceProvider) : base(publishedSnapshotAccessor, publishedModelFactory) {
            _publishedSnapshotAccessor = publishedSnapshotAccessor;
            _serviceProvider = serviceProvider;
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

            // Get the UDIs from the intermediate value
            Udi[] udis = inter as Udi[] ?? Array.Empty<Udi>();

            // Initialize a collection for the items
            List<object> items = new();

            // Determine the item value type
            Type? valueType = config.ValueType;

            // Attempt to get the current published snapshot
            if (_publishedSnapshotAccessor.TryGetPublishedSnapshot(out IPublishedSnapshot? publishedSnapshot)) {

                foreach (Udi udi in udis) {

                    // Look up the media
                    IPublishedContent? media = publishedSnapshot?.Media?.GetById(udi);
                    if (media == null) continue;

                    // If the configuration doesn't specify a value type, we just create a new MediaItem (or relevant sub class)
                    if (valueType == null) {
                        switch (media.ContentType.Alias) {
                            case Constants.Conventions.MediaTypes.Image: items.Add(new ImageItem(media, config)); break;
                            case Constants.Conventions.MediaTypes.File: items.Add(new FileItem(media, config)); break;
                            default: items.Add(new MediaItem(media)); break;
                        }
                        continue;
                    }

                    // If the selected type has a constructor with an ImagePickerConfiguration as the second parameter, we choose that constructor
                    if (HasConstructor<IPublishedContent, MediaPickerConfiguration>(valueType)) {
                        items.Add(ActivatorUtilities.CreateInstance(_serviceProvider, valueType, media, config));
                        continue;
                    }

                    items.Add(ActivatorUtilities.CreateInstance(_serviceProvider, valueType, media));

                }

            }

            // Return the item(s) with the correct value type
            valueType ??= typeof(MediaItem);
            return config is { Multiple: true } ? items.Cast(valueType).ToList(valueType) : items.FirstOrDefault();

        }

        /// <summary>
        /// Returns the type of values returned by the converter.
        /// </summary>
        /// <param name="propertyType">The property type.</param>
        /// <returns>The CLR type of values returned by the converter.</returns>
        public override Type GetPropertyValueType(IPublishedPropertyType propertyType) {

            LimboMediaPickerConfiguration? config = propertyType.DataType.ConfigurationAs<LimboMediaPickerConfiguration>();
            if (config == null) return typeof(MediaItem);

            Type valueType = config.ValueType ?? (config.OnlyImages ? typeof(ImageItem) : typeof(MediaItem));

            return config.Multiple ? typeof(IReadOnlyList<>).MakeGenericType(valueType) : valueType;

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

        #endregion

    }

}