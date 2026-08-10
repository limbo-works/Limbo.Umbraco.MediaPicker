// [CHANGE: Umbraco 13→17 upgrade] Related: all files under src/, see documentation/upgrade-to-umbraco-17.md
using Limbo.Umbraco.MediaPicker.Models;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;

#pragma warning disable CS1591

namespace Limbo.Umbraco.MediaPicker.PropertyEditors;

/// <summary>
/// Configuration editor for the <see cref="LimboMediaPickerEditor"/> property editor. In Umbraco 17 all UI concerns
/// (labels, descriptions and views for the individual fields) are declared client-side in <c>umbraco-package.json</c>,
/// so this class only binds the strongly typed <see cref="LimboMediaPickerConfiguration"/>.
/// </summary>
public class LimboMediaPickerConfigurationEditor : ConfigurationEditor<LimboMediaPickerConfiguration> {

    public LimboMediaPickerConfigurationEditor(IIOHelper ioHelper) : base(ioHelper) { }

}
