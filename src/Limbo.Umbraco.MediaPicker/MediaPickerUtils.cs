using System;
using System.Linq;
using Skybrud.Essentials.Exceptions;
using Skybrud.Essentials.Strings.Extensions;

namespace Limbo.Umbraco.MediaPicker;

internal static class MediaPickerUtils {

    public static string GetTypeAlias(Type type) {
        return type.AssemblyQualifiedName?.Split(',').Take(2).Join(",") ?? throw new ComputerSaysNoException("Failed determining assembly qualified name for item converter.");
    }

}