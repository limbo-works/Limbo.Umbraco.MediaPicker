using System;
using System.Linq;
using Limbo.Umbraco.MediaPicker.Converters;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Skybrud.Essentials.Reflection.Extensions;
using Umbraco.Cms.Web.BackOffice.Controllers;
using Umbraco.Cms.Web.Common.Attributes;
using Umbraco.Cms.Web.Common.Filters;

namespace Limbo.Umbraco.MediaPicker.Controllers.Api;

/// <summary>
/// Umbraco authorized controller for the backoffice plugins
/// </summary>
[AngularJsonOnlyConfiguration]
[PluginController("Limbo")]
public class MediaPickerController : UmbracoAuthorizedApiController {

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
    [HttpGet]
    public object GetConverters() {
        return _mediaPickerConverterCollection.Select(Map);
    }

    #endregion

    #region Private helper methods

    private static JObject Map(IMediaPickerTypeConverter converter) {

        Type type = converter.GetType();

        string icon = $"icon-box color-{type.Assembly.FullName?.Split('.')[0].Split(',')[0].Trim().ToLower()}";
        string name = string.IsNullOrWhiteSpace(converter.Name) ? type.Name : converter.Name;

        JObject json = new() {
            { "assembly", type.Assembly.FullName },
            { "type", converter.Alias },
            { "icon", icon },
            { "name", name },
            { "description", $"{type.AssemblyQualifiedName?.Split(_versionSeparator, StringSplitOptions.None)[0]}.dll" }
        };

        if (type.IsObsolete(out ObsoleteAttribute? obsolete)) {
            json.Add("obsolete", new JObject {
                {"message", obsolete!.Message ?? string.Empty}
            });
        }


        return json;

    }

    #endregion

}