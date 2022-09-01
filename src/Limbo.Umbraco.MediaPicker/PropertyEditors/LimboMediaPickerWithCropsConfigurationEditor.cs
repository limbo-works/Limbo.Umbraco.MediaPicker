using System.Collections.Generic;
using Limbo.Umbraco.MediaPicker.Models;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

#pragma warning disable CS1591

namespace Limbo.Umbraco.MediaPicker.PropertyEditors {

    public class LimboMediaPickerWithCropsConfigurationEditor : ConfigurationEditor<LimboMediaPickerWithCropsConfiguration> {

        public LimboMediaPickerWithCropsConfigurationEditor(IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser) : base(ioHelper, editorConfigurationParser) {
            Field(nameof(MediaPicker3Configuration.StartNodeId))
                .Config = new Dictionary<string, object> { { "idType", "udi" } };

            Field(nameof(MediaPicker3Configuration.Filter))
                .Config = new Dictionary<string, object> { { "itemType", "media" } };

            foreach (var field in Fields) {
                if (field.View is not null) field.View = field.View.Replace("{version}", MediaPickerPackage.InformationalVersion);
            }

        }

        public override IDictionary<string, object> ToValueEditor(object? configuration) {

            var d = base.ToValueEditor(configuration);

            d["idType"] = "udi";
            d["disableFolderSelect"] = "true";
            d["onlyImages"] = "true";

            return d;

        }

    }

}