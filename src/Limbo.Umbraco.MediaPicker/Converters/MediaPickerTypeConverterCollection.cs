using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Umbraco.Cms.Core.Composing;

#pragma warning disable 1591

namespace Limbo.Umbraco.MediaPicker.Converters;

/// <summary>
/// Collection of <see cref="IMediaPickerTypeConverter"/>.
/// </summary>
public sealed class MediaPickerTypeConverterCollection : BuilderCollectionBase<IMediaPickerTypeConverter> {

    private readonly Dictionary<string, IMediaPickerTypeConverter> _lookup;

    public MediaPickerTypeConverterCollection(Func<IEnumerable<IMediaPickerTypeConverter>> items) : base(items) {

        _lookup = new Dictionary<string, IMediaPickerTypeConverter>(StringComparer.OrdinalIgnoreCase);

        foreach (IMediaPickerTypeConverter item in this) {
            _lookup.TryAdd(item.Alias, item);
        }

    }

    public bool TryGet(string typeName, [NotNullWhen(true)] out IMediaPickerTypeConverter? item) {
        return _lookup.TryGetValue(typeName, out item);
    }

}