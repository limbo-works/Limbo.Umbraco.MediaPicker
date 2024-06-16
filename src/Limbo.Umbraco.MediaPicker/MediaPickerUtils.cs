using System;
using System.Linq;
using Skybrud.Essentials.Exceptions;
using Skybrud.Essentials.Strings.Extensions;
using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.MediaPicker;

internal static class MediaPickerUtils {

    public static string GetTypeAlias(Type type) {
        return type.AssemblyQualifiedName?.Split(',').Take(2).Join(",") ?? throw new ComputerSaysNoException("Failed determining assembly qualified name for item converter.");
    }

    public static void PrependLinkToDescription(ConfigurationField field, string text, string url) {
        string a = $"<a href=\"{url}\" class=\"btn btn-primary btn-xs limbo-media-picker-button\" target=\"_blank\" rel=\"noreferrer noopener\">{text}</a>";
        field.Description = $"{a}\r\n{field.Description}";
    }

}