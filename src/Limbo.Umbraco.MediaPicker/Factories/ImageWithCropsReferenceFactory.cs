using System.Collections.Generic;
using System.Linq;
using Limbo.Umbraco.MediaPicker.Json;
using Limbo.Umbraco.MediaPicker.PropertyEditors;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.Models.Editors;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Extensions;

#pragma warning disable CS1591

namespace Limbo.Umbraco.MediaPicker.Factories;

public class ImageWithCropsReferenceFactory : IDataValueReferenceFactory, IDataValueReference {

    private readonly IJsonSerializer _jsonSerializer;

    public ImageWithCropsReferenceFactory(IJsonSerializer jsonSerializer) {
        _jsonSerializer = jsonSerializer;
    }

    /// <inheritdoc />
    public IDataValueReference GetDataValueReference() => this;

    /// <inheritdoc />
    public bool IsForEditor(IDataEditor? dataEditor) {
        return dataEditor != null && dataEditor.Alias.InvariantEquals(LimboMediaPickerWithCropsPropertyEditor.EditorAlias);
    }

    IEnumerable<UmbracoEntityReference> IDataValueReference.GetReferences(object? value) {

        if (value is not string str) return Enumerable.Empty<UmbracoEntityReference>();

        var dtos = MediaWithCropsDeserializer.Deserialize(_jsonSerializer, str);

        return dtos
            .Select(dto => new UmbracoEntityReference(new GuidUdi(Constants.UdiEntityType.Media, dto.MediaKey)))
            .ToList();

    }

}