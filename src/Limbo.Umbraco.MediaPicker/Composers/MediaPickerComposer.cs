using Limbo.Umbraco.MediaPicker.Converters;
using Limbo.Umbraco.MediaPicker.Extensions;
using Limbo.Umbraco.MediaPicker.Factories;
using Limbo.Umbraco.MediaPicker.Manifests;
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.Composing;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Infrastructure.Manifest;

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

        builder.Services.AddSingleton<IPackageManifestReader, MediaPickerManifestReader>();

        builder
            .DataValueReferenceFactories()
            .Append<MediaPickerReferenceFactory>();

        builder
            .WithCollectionBuilder<MediaPickerTypeConverterCollectionBuilder>()
            .Add(() => builder.TypeLoader.GetTypes<IMediaPickerTypeConverter>());

        builder.AddLimboMediaPickerPropertyIndexValueFactory<NoopPropertyIndexValueFactory>();

    }

}