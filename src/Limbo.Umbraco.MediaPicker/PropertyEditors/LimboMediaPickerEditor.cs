// [CHANGE: Umbraco 13→17 upgrade] Related: all files under src/, see documentation/upgrade-to-umbraco-17.md
using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable CS1591

namespace Limbo.Umbraco.MediaPicker.PropertyEditors;

/// <summary>
/// Extends the MediaPicker3 property editor with our additional config options.
/// </summary>
/// <seealso cref="MediaPicker3PropertyEditor" />
[DataEditor(EditorAlias, ValueType = ValueTypes.Json, ValueEditorIsReusable = true)]
public class LimboMediaPickerEditor : MediaPicker3PropertyEditor {

    private readonly IIOHelper _ioHelper;

    #region Constants

    /// <summary>
    /// Gets the alias of the editor (the property editor schema alias in the new backoffice).
    /// </summary>
    public const string EditorAlias = "Limbo.Umbraco.MediaPicker";

    /// <summary>
    /// Gets the name of the editor.
    /// </summary>
    public const string EditorName = "Limbo Media Picker";

    /// <summary>
    /// Gets the alias of the property editor UI shown in the backoffice.
    /// </summary>
    public const string EditorUiAlias = "Limbo.PropertyEditorUi.MediaPicker";

    #endregion

    #region Constructors

    public LimboMediaPickerEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper, [FromKeyedServices(MediaPickerPackage.Alias)] IPropertyIndexValueFactory propertyIndexValueFactory) : base(dataValueEditorFactory, ioHelper) {
        _ioHelper = ioHelper;
        PropertyIndexValueFactory = propertyIndexValueFactory;
        SupportsReadOnly = true;
    }

    #endregion

    #region Member methods

    public override IPropertyIndexValueFactory PropertyIndexValueFactory { get; }

    protected override IConfigurationEditor CreateConfigurationEditor() => new LimboMediaPickerConfigurationEditor(_ioHelper);

    #endregion

}
