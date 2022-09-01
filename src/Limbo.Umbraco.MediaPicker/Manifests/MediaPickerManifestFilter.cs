using System.Collections.Generic;
using Skybrud.Essentials.Strings.Extensions;
using Umbraco.Cms.Core.Manifest;

namespace Limbo.Umbraco.MediaPicker.Manifests {

    /// <inheritdoc />
    public class MediaPickerManifestFilter : IManifestFilter {

        /// <inheritdoc />
        public void Filter(List<PackageManifest> manifests) {
            manifests.Add(new PackageManifest {
                PackageName = MediaPickerPackage.Alias.ToKebabCase(),
                BundleOptions = BundleOptions.Independent,
                Scripts = new[] {
                    $"/App_Plugins/{MediaPickerPackage.Alias}/Scripts/Controllers/TypePicker.js",
                    $"/App_Plugins/{MediaPickerPackage.Alias}/Scripts/Controllers/TypePickerOverlay.js"
                },
                Stylesheets = new[] {
                    $"/App_Plugins/{MediaPickerPackage.Alias}/Styles/Styles.css"
                }
            });
        }

    }

}