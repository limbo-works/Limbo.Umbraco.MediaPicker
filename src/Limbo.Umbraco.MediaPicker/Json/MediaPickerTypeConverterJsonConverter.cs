using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using Limbo.Umbraco.MediaPicker.Models;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Limbo.Umbraco.MediaPicker.Json;

public class MediaPickerTypeConverterJsonConverter : JsonConverter<MediaPickerTypeConverter> {

    public override MediaPickerTypeConverter? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) {

        switch (reader.TokenType) {

            case JsonTokenType.Null:
                return null;

            // Legacy format where the value was saved as the raw type name
            case JsonTokenType.String: {
                string? type = reader.GetString();
                return Create(type);
            }

            case JsonTokenType.StartObject: {
                using JsonDocument document = JsonDocument.ParseValue(ref reader);
                string? type = document.RootElement.TryGetProperty("type", out JsonElement element) && element.ValueKind == JsonValueKind.String ? element.GetString() : null;
                return Create(type);
            }

            default:
                throw new JsonException($"Unsupported token type: {reader.TokenType}...");

        }

    }

    public override void Write(Utf8JsonWriter writer, MediaPickerTypeConverter? value, JsonSerializerOptions options) {

        if (value is not null && !string.IsNullOrWhiteSpace(value.Type)) {
            writer.WriteStartObject();
            writer.WriteString("type", value.Type);
            writer.WriteEndObject();
            return;
        }

        writer.WriteNullValue();

    }

    // [CHANGE: QA review fix] Related: Limbo.Umbraco.MediaPicker.csproj, wwwroot/limbo-media-picker-type-converter.element.js
    // Very old versions saved the value as the full assembly qualified type name (including "Version=", "Culture="
    // etc.), whereas the collection is keyed by the shorter alias returned by MediaPickerUtils.GetTypeAlias. The
    // AngularJS view used to rewrite the value client side - since that view is gone, we normalize it here instead,
    // as the selected converter would otherwise silently not be found.
    private static MediaPickerTypeConverter? Create(string? type) {
        if (string.IsNullOrWhiteSpace(type)) return null;
        int index = type.IndexOf(", Version", StringComparison.Ordinal);
        if (index > 0) type = type[..index];
        return string.IsNullOrWhiteSpace(type) ? null : new MediaPickerTypeConverter(type);
    }

}
