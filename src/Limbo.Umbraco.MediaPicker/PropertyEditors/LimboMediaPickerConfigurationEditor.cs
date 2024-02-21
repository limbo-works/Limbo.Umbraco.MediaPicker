using System.Collections.Generic;
using Limbo.Umbraco.MediaPicker.Models;
using Umbraco.Cms.Core.IO;
using Umbraco.Cms.Core.PropertyEditors;
using Umbraco.Cms.Core.Services;

#pragma warning disable CS1591

namespace Limbo.Umbraco.MediaPicker.PropertyEditors;

public class LimboMediaPickerConfigurationEditor : ConfigurationEditor<LimboMediaPickerConfiguration> {

    public LimboMediaPickerConfigurationEditor(IIOHelper ioHelper, IEditorConfigurationParser editorConfigurationParser) : base(ioHelper, editorConfigurationParser) {

        foreach (var field in Fields) {

            if (field.View is not null) field.View = field.View.Replace("{version}", MediaPickerPackage.InformationalVersion);

            switch (field.Key) {

                case "valueType":
                    MediaPickerUtils.PrependLinkToDescription(
                        field,
                        "See the documentation &rarr;",
                        "https://packages.limbo.works/ee815b6f"
                    );
                    break;

            }

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