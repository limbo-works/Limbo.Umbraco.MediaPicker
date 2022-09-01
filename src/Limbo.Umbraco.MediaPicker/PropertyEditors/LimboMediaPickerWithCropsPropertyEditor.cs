using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.Models;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

#pragma warning disable CS1591

namespace Limbo.Umbraco.MediaPicker.PropertyEditors {

    /// <summary>
    /// Extends the MediaPicker3 property editor with our additional config options
    /// </summary>
    /// <seealso cref="MediaPicker3PropertyEditor" />
    [DataEditor(EditorAlias, EditorType.PropertyValue, EditorName, EditorView, Group = EditorGroup, Icon = EditorIcon, ValueType = ValueTypes.Text)]
    public class LimboMediaPickerWithCropsPropertyEditor : MediaPicker3PropertyEditor {

        private readonly IIOHelper _iOHelper;
        private readonly IEditorConfigurationParser _editorConfigurationParser;

        #region Constants

        /// <summary>
        /// Gets the alias of the editor.
        /// </summary>
        public const string EditorAlias = "Limbo.Umbraco.MediaPicker3";

        /// <summary>
        /// Gets the name of the editor.
        /// </summary>
        public const string EditorName = "Limbo Media Picker v3";

        /// <summary>
        /// Gets the group name of the editor.
        /// </summary>
        public const string EditorGroup = "Limbo";

        /// <summary>
        /// Gets the icon of the editor.
        /// </summary>
        public const string EditorIcon = "icon-picture color-limbo";

        /// <summary>
        /// Gets the view of the editor.
        /// </summary>
        public const string EditorView = "mediapicker3";

        #endregion

        #region Constructors

        #endregion

        #region Member methods

        public LimboMediaPickerWithCropsPropertyEditor(IDataValueEditorFactory dataValueEditorFactory, IIOHelper iOHelper, IEditorConfigurationParser editorConfigurationParser) : base(dataValueEditorFactory, iOHelper, editorConfigurationParser) {
            _iOHelper = iOHelper;
            _editorConfigurationParser = editorConfigurationParser;
        }

        protected override IConfigurationEditor CreateConfigurationEditor() => new LimboMediaPickerWithCropsConfigurationEditor(_iOHelper, _editorConfigurationParser);
        
        public override IDataValueEditor GetValueEditor(object? configuration) {

            IDataValueEditor editor = base.GetValueEditor(configuration);
            if (editor is not DataValueEditor dve) return editor;

            if (dve.View is not null) {
                dve.View = dve.View.Replace("{version}", MediaPickerPackage.SemVersion.ToString());
            }

            return editor;

        }

        #endregion

    }

}