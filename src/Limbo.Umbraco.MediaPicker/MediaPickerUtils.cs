// [CHANGE: Umbraco 13→17 upgrade] Related: all files under src/, see documentation/upgrade-to-umbraco-17.md
// PrependLinkToDescription removed - field descriptions are now declared client-side in umbraco-package.json.
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
