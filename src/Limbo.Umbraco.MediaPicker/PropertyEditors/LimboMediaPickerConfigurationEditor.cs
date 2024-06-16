using System.Collections.Generic;
using Limbo.Umbraco.MediaPicker.Models;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

#pragma warning disable CS1591

namespace Limbo.Umbraco.MediaPicker.PropertyEditors;

public class LimboMediaPickerConfigurationEditor : ConfigurationEditor<LimboMediaPickerConfiguration> {

    public LimboMediaPickerConfigurationEditor(IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser) : base(ioHelper, editorConfigurationParser) {

        Field(nameof(MediaPicker3Configuration.StartNodeId))
            .Config = new Dictionary<string, object> { { "idType", "udi" } };

        Field(nameof(MediaPicker3Configuration.Filter))
            .Config = new Dictionary<string, object> { { "itemType", "media" } };

        foreach (ConfigurationField field in Fields) {

            if (field.View is not null) field.View = field.View.Replace("{version}", MediaPickerPackage.InformationalVersion);

            switch (field.Key) {

                case "itemConverter":
                    MediaPickerUtils.PrependLinkToDescription(
                        field,
                        "See the documentation &rarr;",
                        "https://packages.limbo.works/e7b725c2"
                    );
                    break;

                case "multiple":
                    field.Description = "Outputs an <strong>IReadOnlyList&lt;T&gt;</strong> instead of <strong>T</strong> if enabled.";
                    break;

            }

        }

        Fields.Insert(0, new ConfigurationField {
            Key = "advancedSeparator",
            Name = "Advanced Options",
            View = $"/App_Plugins/{MediaPickerPackage.Alias}/Views/Separator.html",
            HideLabel = true
        });

        Fields.Insert(2, new ConfigurationField {
            Key = "defaultSeparator",
            Name = "Default Options",
            View = $"/App_Plugins/{MediaPickerPackage.Alias}/Views/Separator.html",
            HideLabel = true
        });

    }

    public override IDictionary<string, object> ToValueEditor(object? configuration) {

        var d = base.ToValueEditor(configuration);

        d["idType"] = "udi";
        d["disableFolderSelect"] = "true";
        d["onlyImages"] = "true";

        return d;

    }

}