// [CHANGE: Umbraco 13→17 upgrade] Related: all files under src/, see documentation/upgrade-to-umbraco-17.md
// IManifestFilter no longer exists - the package manifest is now the static wwwroot/umbraco-package.json file.
using Limbo.Umbraco.MediaPicker.Converters;
using Limbo.Umbraco.MediaPicker.Extensions;
using Limbo.Umbraco.MediaPicker.Factories;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.PropertyEditors;

namespace Limbo.Umbraco.MediaPicker.Composers;

/// <summary>
/// Composer to run our reference factories when the site starts up.
/// </summary>
public class ImagePickerComposer : IComposer {

    /// <summary>
    /// Append reference factories on startup.
    /// </summary>
    /// <param name="builder">Umbraco's own injected builder that runs on startup.</param>
    public void Compose(IUmbracoBuilder builder) {

        builder
            .DataValueReferenceFactories()
            .Append<MediaPickerReferenceFactory>();

        builder
            .WithCollectionBuilder<MediaPickerTypeConverterCollectionBuilder>()
            .Add(() => builder.TypeLoader.GetTypes<IMediaPickerTypeConverter>());

        builder.AddLimboMediaPickerPropertyIndexValueFactory<NoopPropertyIndexValueFactory>();

    }

}
