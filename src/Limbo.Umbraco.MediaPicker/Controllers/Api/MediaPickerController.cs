using System;
using System.Collections.Generic;
using System.Linq;
using Asp.Versioning;
using Limbo.Umbraco.MediaPicker.Api;
using Limbo.Umbraco.MediaPicker.Converters;
using Limbo.Umbraco.MediaPicker.Models.Api;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Skybrud.Essentials.Reflection.Extensions;
using Umbraco.Cms.Api.Common.Attributes;
using Umbraco.Cms.Api.Management.Controllers;
using Umbraco.Cms.Api.Management.Routing;
using Umbraco.Cms.Web.Common.Authorization;

namespace Limbo.Umbraco.MediaPicker.Controllers.Api;

/// <summary>
/// Management API controller used by the backoffice UI of the package.
/// </summary>
[ApiController]
[VersionedApiBackOfficeRoute(MediaPickerApiConstants.Route)]
[Authorize(Policy = AuthorizationPolicies.SectionAccessContent)]
[MapToApi(MediaPickerApiConstants.Alias)]
[ApiVersion("1.0")]
[ApiExplorerSettings(GroupName = MediaPickerApiConstants.GroupName)]
public class MediaPickerController : ManagementApiControllerBase {

    private static readonly string[] _versionSeparator = [", Version"];

    private readonly MediaPickerTypeConverterCollection _mediaPickerConverterCollection;

    #region Constructors

    /// <summary>
    /// Initializes a new instance based on the specified dependency.
    /// </summary>
    /// <param name="mediaPickerConverterCollection"></param>
    public MediaPickerController(MediaPickerTypeConverterCollection mediaPickerConverterCollection) {
        _mediaPickerConverterCollection = mediaPickerConverterCollection;
    }

    #endregion

    #region Public API methods

    /// <summary>
    /// Returns a list of all converters for the media picker property editor.
    /// </summary>
    /// <returns>A list of available converters.</returns>
    [HttpGet("converters")]
    [ProducesResponseType<IEnumerable<MediaPickerTypeConverterModel>>(StatusCodes.Status200OK)]
    public IActionResult GetConverters() {
        return Ok(_mediaPickerConverterCollection.Select(Map));
    }

    #endregion

    #region Private helper methods

    private static MediaPickerTypeConverterModel Map(IMediaPickerTypeConverter converter) {

        Type type = converter.GetType();

        string icon = $"icon-box color-{type.Assembly.FullName?.Split('.')[0].Split(',')[0].Trim().ToLower()}";
        string name = string.IsNullOrWhiteSpace(converter.Name) ? type.Name : converter.Name;

        return new MediaPickerTypeConverterModel {
            Assembly = type.Assembly.FullName,
            Type = converter.Alias,
            Icon = icon,
            Name = name,
            Description = $"{type.AssemblyQualifiedName?.Split(_versionSeparator, StringSplitOptions.None)[0]}.dll",
            Obsolete = type.IsObsolete(out ObsoleteAttribute? obsolete) ? new MediaPickerTypeConverterObsoleteModel { Message = obsolete.Message } : null
        };

    }

    #endregion

}