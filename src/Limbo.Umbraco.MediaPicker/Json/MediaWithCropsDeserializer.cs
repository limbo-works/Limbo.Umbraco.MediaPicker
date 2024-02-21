using System;
using System.Collections.Generic;
using System.Linq;
using Limbo.Umbraco.MediaPicker.Models;
using Umbraco.Cms.Core;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;
using Umbraco.Cms.Core.Serialization;
using Umbraco.Extensions;

namespace Limbo.Umbraco.MediaPicker.Json;

/// <summary>
/// Helper class to deserialize a json blob into the MediaWithCrops model
/// </summary>
public class MediaWithCropsDeserializer {

    internal static IEnumerable<MediaWithCropsDto> Deserialize(IJsonSerializer jsonSerializer, object? value) {

        if (value == null) yield break;

        string? rawJson = value as string ?? value?.ToString();
        if (string.IsNullOrWhiteSpace(rawJson)) yield break;

        // Old comma seperated UDI format
        if (!rawJson.DetectIsJson()) {
            foreach (string udiStr in rawJson.Split(Constants.CharArrays.Comma)) {
                if (UdiParser.TryParse(udiStr, out GuidUdi? udi)) {
                    yield return new MediaWithCropsDto {
                        Key = Guid.NewGuid(),
                        MediaKey = udi!.Guid,
                        Crops = Enumerable.Empty<ImageCropperValue.ImageCropperCrop>(),
                        FocalPoint = new ImageCropperValue.ImageCropperFocalPoint {
                            Left = 0.5m,
                            Top = 0.5m
                        }
                    };
                }
            }

            yield break;

        }

        // New JSON format
        var dtos = jsonSerializer.Deserialize<IEnumerable<MediaWithCropsDto>>(rawJson);
        if (dtos == null) yield break;
        foreach (var dto in dtos) {
            yield return dto;
        }

    }

}