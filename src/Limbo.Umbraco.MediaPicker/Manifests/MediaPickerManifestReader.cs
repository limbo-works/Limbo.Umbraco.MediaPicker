using System.Collections.Generic;
using System.Threading.Tasks;
using Limbo.Umbraco.MediaPicker.PropertyEditors;
using Skybrud.Essentials.Security.Extensions;
using Umbraco.Cms.Core.Manifest;
using Umbraco.Cms.Infrastructure.Manifest;

#pragma warning disable CS1591 // Missing XML comment for publicly visible type or member

namespace Limbo.Umbraco.MediaPicker.Manifests;

public class MediaPickerManifestReader : IPackageManifestReader {

    protected string CacheBuster { get; } = MediaPickerPackage.InformationalVersion.ToMd5Hash();

    public Task<IEnumerable<PackageManifest>> ReadPackageManifestsAsync() {

        List<PackageManifest> list = [
            new() {
                AllowTelemetry = true,
                Id = MediaPickerPackage.Alias,
                Name = MediaPickerPackage.Name,
                Version = MediaPickerPackage.InformationalVersion,
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
        return new        {
            type = "propertyEditorSchema",
            name = "Limbo Media Picker Schema",
            alias = LimboMediaPickerPropertyEditor.EditorAlias,
            meta = new            {
                defaultPropertyEditorUiAlias = LimboMediaPickerPropertyEditor.EditorUiAlias,
                settings = new                {
                    properties = new object[] {
                        new {
                            alias = "typeConverter",
                            label = "Type converter",
                            description = "Select a type converter, which will be used for converting the default media picker value.\n\n[See the documentation →](https://packages.limbo.works/e7b725c2)",
                            propertyEditorUiAlias = "Limbo.PropertyEditorUi.MediaPickerTypeConverter"
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
            alias = LimboMediaPickerPropertyEditor.EditorUiAlias,
            name = "Limbo Media Picker Property Editor UI",
            element = $"/App_Plugins/{MediaPickerPackage.Alias}/limbo-media-picker.element.js?v={CacheBuster}",
            elementName = "limbo-media-picker",
            meta = new {
                label = "Limbo Media Picker",
                propertyEditorSchemaAlias = LimboMediaPickerPropertyEditor.EditorAlias,
                icon = "icon-picture",
                group = "media",
                supportsReadOnly = true
            }
        };
    }

    private object GetMediaPickerTypeConverterPropertyEditorUi() {
        return new {
            type = "propertyEditorUi",
            alias = LimboMediaPickerConfigurationEditor.EditorUiAlias,
            name = "Limbo Media Picker Type Converter Property Editor UI",
            element = $"/App_Plugins/{MediaPickerPackage.Alias}/limbo-media-picker-type-converter.element.js?v={CacheBuster}",
            elementName = "limbo-media-picker-type-converter",
            meta = new {
                label = "Limbo Media Picker Type Converter",
                propertyEditorSchemaAlias = LimboMediaPickerPropertyEditor.EditorAlias,
                icon = "icon-picture",
                group = "media"
            }
        };
    }

}