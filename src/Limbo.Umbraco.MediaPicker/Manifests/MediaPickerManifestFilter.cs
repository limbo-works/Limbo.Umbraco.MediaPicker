using System.Collections.Generic;
using System.Reflection;
using Umbraco.Cms.Core.Manifest;

namespace Limbo.Umbraco.MediaPicker.Manifests;

/// <inheritdoc />
public class MediaPickerManifestFilter : IManifestFilter {

    /// <inheritdoc />
    public void Filter(List<PackageManifest> manifests) {

        // Initialize a new manifest filter for this package
        PackageManifest manifest = new() {
            AllowPackageTelemetry = true,
            PackageName = MediaPickerPackage.Name,
            Version = MediaPickerPackage.InformationalVersion,
            BundleOptions = BundleOptions.Independent,
            Scripts = new[] {
                $"/App_Plugins/{MediaPickerPackage.Alias}/Scripts/Controllers/TypePicker.js",
                $"/App_Plugins/{MediaPickerPackage.Alias}/Scripts/Controllers/TypePickerOverlay.js"
            },
            Stylesheets = new[] {
                $"/App_Plugins/{MediaPickerPackage.Alias}/Styles/Styles.css"
            }
        };

        // The "PackageId" property isn't available prior to Umbraco 12, and since the package is build against
        // Umbraco 10, we need to use reflection for setting the property value for Umbraco 12+. Ideally this
        // shouldn't fail, but we might at least add a try/catch to be sure
        try {
            PropertyInfo? property = manifest.GetType().GetProperty("PackageId");
            property?.SetValue(manifest, MediaPickerPackage.Alias);
        } catch {
            // We don't really care about the exception
        }

        // Append the manifest
        manifests.Add(manifest);

    }

}