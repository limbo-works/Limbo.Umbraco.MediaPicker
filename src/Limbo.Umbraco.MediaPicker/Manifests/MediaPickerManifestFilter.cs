using System.Collections.Generic;
using Umbraco.Cms.Core.Manifest;

namespace Limbo.Umbraco.MediaPicker.Manifests;

/// <inheritdoc />
public class MediaPickerManifestFilter : IManifestFilter {

    /// <inheritdoc />
    public void Filter(List<PackageManifest> manifests) {

        // Initialize a new manifest filter for this package
        PackageManifest manifest = new() {
            AllowPackageTelemetry = true,
            PackageId = MediaPickerPackage.Alias,
            PackageName = MediaPickerPackage.Name,
            Version = MediaPickerPackage.InformationalVersion,
            BundleOptions = BundleOptions.Independent,
            Scripts = [
                $"/App_Plugins/{MediaPickerPackage.Alias}/Scripts/Controllers/ItemConverter.js",
                $"/App_Plugins/{MediaPickerPackage.Alias}/Scripts/Controllers/ItemConverterOverlay.js"
            ],
            Stylesheets = [
                $"/App_Plugins/{MediaPickerPackage.Alias}/Styles/Styles.css"
            ]
        };

        // Append the manifest
        manifests.Add(manifest);

    }

}