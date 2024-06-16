using Limbo.Umbraco.MediaPicker.Converters;
using Limbo.Umbraco.MediaPicker.Factories;
using Limbo.Umbraco.MediaPicker.Manifests;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;

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
            .WithCollectionBuilder<MediaPickerItemConverterCollectionBuilder>()
            .Add(() => builder.TypeLoader.GetTypes<IMediaPickerItemConverter>());

        builder
            .ManifestFilters()
            .Append<MediaPickerManifestFilter>();

    }

}