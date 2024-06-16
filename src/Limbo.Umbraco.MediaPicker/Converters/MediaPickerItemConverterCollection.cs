using System;
using System.Linq;
using System.Collections.Generic;
using Umbraco.Cms.Core.Composing;

#pragma warning disable 1591

namespace Limbo.Umbraco.MediaPicker.Converters;

/// <summary>
/// Collection of <see cref="IMediaPickerItemConverter"/>.
/// </summary>
public sealed class MediaPickerItemConverterCollection : BuilderCollectionBase<IMediaPickerItemConverter> {

    private readonly Dictionary<string, IMediaPickerItemConverter> _lookup;

    public IReadOnlyList<string> Keys => _lookup.Keys.ToArray();

    public MediaPickerItemConverterCollection(Func<IEnumerable<IMediaPickerItemConverter>> items) : base(items) {

        _lookup = new Dictionary<string, IMediaPickerItemConverter>(StringComparer.OrdinalIgnoreCase);

        foreach (IMediaPickerItemConverter item in this) {
            _lookup.TryAdd(item.Alias, item);
        }

    }

    public bool TryGet(string typeName, out IMediaPickerItemConverter? item) {
        return _lookup.TryGetValue(typeName, out item);
    }

}