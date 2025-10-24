using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.DependencyInjection;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Limbo.Umbraco.MediaPicker.Extensions;

public static class MediaPickerExtensions {

    public static IUmbracoBuilder AddLimboMediaPickerPropertyIndexValueFactory<TFactory>(this IUmbracoBuilder builder) where TFactory : class, IPropertyIndexValueFactory {
        builder.Services.AddKeyedSingleton<IPropertyIndexValueFactory, TFactory>(MediaPickerPackage.Alias);
        return builder;
    }

}