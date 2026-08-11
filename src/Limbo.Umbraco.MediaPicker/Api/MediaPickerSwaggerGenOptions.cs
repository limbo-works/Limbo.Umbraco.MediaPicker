using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Limbo.Umbraco.MediaPicker.Api;

#pragma warning disable CS1591

public class MediaPickerSwaggerGenOptions : IConfigureOptions<SwaggerGenOptions> {

    public void Configure(SwaggerGenOptions options) {
        options.SwaggerDoc(MediaPickerApiConstants.Alias, new OpenApiInfo {
            Title = MediaPickerApiConstants.Name,
            Version = "1.0"
        });
        options.OperationFilter<MediaPickerSecurityFilter>();
    }

}