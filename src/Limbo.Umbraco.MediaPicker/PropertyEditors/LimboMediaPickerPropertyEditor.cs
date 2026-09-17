using Microsoft.Extensions.DependencyInjection;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable CS1591

namespace Limbo.Umbraco.MediaPicker.PropertyEditors;

/// <summary>
/// Extends the MediaPicker3 property editor with our additional config options.
/// </summary>
/// <seealso cref="MediaPicker3PropertyEditor" />
[DataEditor(EditorAlias, ValueType = EditorValueType, ValueEditorIsReusable = true)]
public class LimboMediaPickerPropertyEditor : MediaPicker3PropertyEditor {

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
    public const string EditorUiAlias = "Limbo.Umbraco.MediaPicker.PropertyEditorUi";

    /// <summary>
    /// Gets the icon alias of the property editor.
    /// </summary>
    public const string EditorIcon = "icon-picture";

    /// <summary>
    /// Gets the group of the property editor.
    /// </summary>
    public const string EditorGroup = "Limbo";

    /// <summary>
    /// Gets the value type of the property editor.
    /// </summary>
    public const string EditorValueType = ValueTypes.Json;

    #endregion

    #region Constructors

    public LimboMediaPickerPropertyEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper ioHelper, [FromKeyedServices(MediaPickerPackage.Alias)] IPropertyIndexValueFactory propertyIndexValueFactory) : base(dataValueEditorFactory, ioHelper) {
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