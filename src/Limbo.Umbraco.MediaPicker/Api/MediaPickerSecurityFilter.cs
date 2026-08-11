using Umbraco.Cms.Api.Management.OpenApi;

namespace Limbo.Umbraco.MediaPicker.Api;

#pragma warning disable CS1591

public class MediaPickerSecurityFilter : BackOfficeSecurityRequirementsOperationFilterBase {

    protected override string ApiName => MediaPickerApiConstants.Name;

}