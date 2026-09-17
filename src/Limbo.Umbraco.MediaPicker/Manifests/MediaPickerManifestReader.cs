using System.Collections.Generic;
using System.Threading.Tasks;
using Limbo.Umbraco.MediaPicker.Constants;
using Limbo.Umbraco.MediaPicker.PropertyEditors;
using Umbraco.Cms.Core.Manifest;
using Umbraco.Cms.Infrastructure.Manifest;

using static Limbo.Umbraco.MediaPicker.MediaPickerPackage;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Limbo.Umbraco.MediaPicker.Manifests;

public class MediaPickerManifestReader : IPackageManifestReader {

    public Task<IEnumerable<PackageManifest>> ReadPackageManifestsAsync() {

        List<PackageManifest> list = [
            new() {
                AllowTelemetry = true,
                Id = Alias,
                Name = Name,
                Version = InformationalVersion,
                AllowPublicAccess = false,
                Extensions = [
                    GetPropertyEditorSchema(),
                    GetMediaPickerPropertyEditorUi(),
                    GetMediaPickerTypeConverterPropertyEditorUi()
                ],
                Importmap = null,
            }
        ];

        return Task.FromResult<IEnumerable<PackageManifest>>(list);

    }

    private static object GetPropertyEditorSchema() {
        return new {
            type = "propertyEditorSchema",
            name = $"{Name}: Property Editor Schema",
            alias = MediaPickerPropertyEditorSchemaAliases.MediaPicker,
            meta = new            {
                defaultPropertyEditorUiAlias = MediaPickerPropertyEditorUiAliases.MediaPicker,
                settings = new                {
                    properties = new object[] {
                        new {
                            alias = "typeConverter",
                            label = "Type converter",
                            description = "Select a type converter, which will be used for converting the default media picker value.\n\n[See the documentation →](https://packages.limbo.works/e7b725c2)",
                            propertyEditorUiAlias = MediaPickerPropertyEditorUiAliases.TypeConverter
                        },
                        new {
                            alias = "filter",
                            label = "Accepted types",
                            description = "Limit to specific types",
                            propertyEditorUiAlias = "Umb.PropertyEditorUi.MediaTypePicker"
                        },
                        new {
                            alias = "multiple",
                            label = "Pick multiple items",
                            description = "Outputs an `IReadOnlyList<T>` instead of `T` if enabled.",
                            propertyEditorUiAlias = "Umb.PropertyEditorUi.Toggle"
                        },
                        new {
                            alias = "validationLimit",
                            label = "Amount",
                            description = "Set a required range of medias",
                            propertyEditorUiAlias = "Umb.PropertyEditorUi.NumberRange",
                            config = new[] {
                                new {
                                    alias = "validationRange",
                                    value = new { min = 0 }
                                }
                            }
                        },
                        new {
                            alias = "startNodeId",
                            label = "Start node",
                            propertyEditorUiAlias = "Umb.PropertyEditorUi.MediaEntityPicker",
                            config = new[] {
                                new {
                                    alias = "validationLimit",
                                    value = new { min = 0, max = 1 }
                                }
                            }
                        },
                        new {
                            alias = "enableLocalFocalPoint",
                            label = "Enable Focal Point",
                            propertyEditorUiAlias = "Umb.PropertyEditorUi.Toggle"
                        },
                        new {
                            alias = "crops",
                            label = "Image Crops",
                            description = "Local crops, stored on document",
                            propertyEditorUiAlias = "Umb.PropertyEditorUi.ImageCropsConfiguration"
                        },
                        new {
                            alias = "ignoreUserStartNodes",
                            label = "Ignore User Start Nodes",
                            description = "Selecting this option allows a user to choose nodes that they normally dont have access to.",
                            propertyEditorUiAlias = "Umb.PropertyEditorUi.Toggle"
                        }
                    }
                }
            }
        };
    }

    private object GetMediaPickerPropertyEditorUi() {
        return new {
            type = "propertyEditorUi",
            alias = MediaPickerPropertyEditorUiAliases.MediaPicker,
            name = $"{Name}: Property Editor UI",
            element = $"/App_Plugins/{Alias}/limbo-media-picker.element.js",
            elementName = "limbo-media-picker",
            meta = new {
                label = LimboMediaPickerPropertyEditor.EditorName,
                propertyEditorSchemaAlias = MediaPickerPropertyEditorSchemaAliases.MediaPicker,
                icon = LimboMediaPickerPropertyEditor.EditorIcon,
                group = LimboMediaPickerPropertyEditor.EditorGroup,
                supportsReadOnly = true
            }
        };
    }

    private object GetMediaPickerTypeConverterPropertyEditorUi() {
        return new {
            type = "propertyEditorUi",
            alias = MediaPickerPropertyEditorUiAliases.TypeConverter,
            name = $"{Name}: Type Converter Property Editor UI",
            element = $"/App_Plugins/{Alias}/limbo-media-picker-type-converter.element.js",
            elementName = "limbo-media-picker-type-converter",
            meta = new {
                label = "Limbo Media Picker Type Converter",
                //propertyEditorSchemaAlias = MediaPickerPropertyEditorSchemaAliases.MediaPicker,
                icon = "icon-picture",
                group = "Limbo"
            }
        };
    }

}