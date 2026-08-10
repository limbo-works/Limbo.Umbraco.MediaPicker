// [CHANGE: Umbraco 13→17 upgrade] Related: all files under src/, see documentation/upgrade-to-umbraco-17.md
// DataContract/DataMember replaced with System.Text.Json attributes.
using System;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using Umbraco.Cms.Core.PropertyEditors.ValueConverters;

namespace Limbo.Umbraco.MediaPicker.Models;

internal class MediaWithCropsDto {

    [JsonPropertyName("key")]
    [Newtonsoft.Json.JsonProperty("key")]
    public Guid Key { get; set; }

    [JsonPropertyName("mediaKey")]
    [Newtonsoft.Json.JsonProperty("mediaKey")]
    public Guid MediaKey { get; set; }

    [JsonPropertyName("crops")]
    [Newtonsoft.Json.JsonProperty("crops")]
    public IEnumerable<ImageCropperValue.ImageCropperCrop> Crops { get; set; } = null!;

    [JsonPropertyName("focalPoint")]
    [Newtonsoft.Json.JsonProperty("focalPoint")]
    public ImageCropperValue.ImageCropperFocalPoint FocalPoint { get; set; } = null!;

}
