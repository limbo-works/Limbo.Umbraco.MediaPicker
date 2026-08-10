# Upgrade recap: Umbraco 13 → Umbraco 17

This document describes the changes made when upgrading **Limbo.Umbraco.MediaPicker** from Umbraco 13 (.NET 8) to Umbraco 17 (.NET 10), performed on the `v17/dev` branch (July 2026).

The upgrade spans the largest breaking change in Umbraco's history: the v14 backoffice rewrite (AngularJS → Lit/Web Components), the v15 published-cache rework, and the platform-wide switch from Newtonsoft.Json to System.Text.Json.

## Project file (`Limbo.Umbraco.MediaPicker.csproj`)

- Target framework `net8.0` → `net10.0`, C# language version `12.0` → `14.0`.
- Version bumped `13.0.1` → `17.0.0`.
- NuGet dependencies changed to the requested range **`[17.0.0,17.9.9)`**:
  - Removed `Umbraco.Cms.Web.Website` and `Umbraco.Cms.Web.BackOffice` (the latter no longer exists in v14+).
  - Added `Umbraco.Cms.Infrastructure` (base value converter, Delivery API builder) and `Umbraco.Cms.Api.Management` (Management API controller).
  - Kept `Umbraco.Cms.Core` and `Skybrud.Essentials`.
- Added `<NuGetAuditMode>direct</NuGetAuditMode>` so transitive vulnerability advisories pulled in by Umbraco itself don't fail/spam the build (extends the existing `NoWarn="NU1902"` intent).
- Removed the `compilerconfig.json` exclusion (file deleted — no more LESS compilation).
- Package/documentation URLs updated from `/v13/` to `/v17/`.

## Server-side (C#) changes

### Property editor (`PropertyEditors/LimboMediaPickerEditor.cs`)
- `[DataEditor]` attribute reduced to `(alias, ValueType, ValueEditorIsReusable)` — `EditorType`, name, view, icon and group no longer exist server-side; they moved to the client manifest.
- Constructor updated to the v17 base signature `MediaPicker3PropertyEditor(IDataValueEditorFactory, IIOHelper)` — `IEditorConfigurationParser` was removed in v14.
- Removed the `{version}` view cache-busting logic and the `EditorView`/`EditorIcon`/`EditorGroup` constants (no views anymore). Added an `EditorUiAlias` constant (`Limbo.PropertyEditorUi.MediaPicker`).
- The keyed `IPropertyIndexValueFactory` injection (`AddLimboMediaPickerPropertyIndexValueFactory<T>()`) still works unchanged.

### Configuration (`Models/LimboMediaPickerConfiguration.cs`, `PropertyEditors/LimboMediaPickerConfigurationEditor.cs`)
- `[ConfigurationField]` is now key-only — labels, descriptions and views for fields are declared client-side.
- The configuration editor shrank to just binding the typed configuration; all v13 field tweaks (separators, descriptions, `ToValueEditor` hints like `idType: udi`) were UI concerns now handled by the new backoffice.
- MessagePack `[IgnoreMember]` attributes replaced with System.Text.Json `[JsonIgnore]` on the computed `CropMode`/`PreferFocalPoint` properties.

### Value converter (`PropertyEditors/ValueConverters/LimboMediaPickerValueConverter.cs`)
- Constructor updated for v17: `IPublishedSnapshotAccessor` (removed in v15) replaced by `IPublishedMediaCache`.
- `PublishedDataType.Configuration` no longer exists — replaced with `ConfigurationAs<T>()`.
- Conversion logic (running the selected `IMediaPickerTypeConverter` on top of the base conversion) is unchanged.

### Newtonsoft.Json → System.Text.Json
- `Json/Newtonsoft/MediaPickerTypeConverterJsonConverter.cs` (Newtonsoft) replaced by `Json/MediaPickerTypeConverterJsonConverter.cs` (System.Text.Json). It still accepts both the object format `{ "type": "..." }` and the legacy raw-string format.
- All models (`MediaItem`, `FileItem`, `ImageItem`, `MediaWithCropsItem`, `ImageWithCropsItem`) migrated from `[JsonProperty("x", Order = n)]` to `[JsonPropertyName("x")]` + `[JsonPropertyOrder(n)]`.
- `MediaWithCropsDto` migrated from `[DataContract]`/`[DataMember]` to `[JsonPropertyName]` (Umbraco's `IJsonSerializer` is System.Text.Json-based in v14+).
- `[JsonProperty]` attributes removed from `IMediaPickerTypeConverter` — the API controller now maps converters to a typed response model instead of serializing them directly.

### API controller (`Controllers/Api/MediaPickerController.cs`)
- `UmbracoAuthorizedApiController` was removed in v14. The controller is now a **Management API controller** (`ManagementApiControllerBase` + `[VersionedApiBackOfficeRoute]`).
- Old route: `/umbraco/backoffice/Limbo/MediaPicker/GetConverters`
- New route: `GET /umbraco/management/api/v1/limbo/media-picker/converters`
- The Newtonsoft `JObject` response replaced by a typed model (`Models/Api/MediaPickerTypeConverterModel.cs`) with the same JSON shape (`type`, `name`, `icon`, `assembly`, `description`, `obsolete.message`).

### Composer (`Composers/MediaPickerComposer.cs`)
- `IManifestFilter`/`ManifestFilters()` no longer exist — `Manifests/MediaPickerManifestFilter.cs` deleted. The package is now announced via the static `wwwroot/umbraco-package.json` file.
- Reference factory, converter collection and keyed index-value-factory registrations unchanged.

### Misc
- `MediaPickerPackage.SemVersion` removed (only used for view cache busting); `MediaPickerUtils.PrependLinkToDescription` removed (`ConfigurationField.Description` no longer exists server-side).
- `debug.bat` output path updated `c:\nuget\Umbraco13` → `c:\nuget\Umbraco17`.

## Client-side: AngularJS → Lit (the big one)

All AngularJS-era assets were **deleted**:

| Deleted | Reason |
|---|---|
| `wwwroot/Scripts/Controllers/TypeConverter.js` + `TypeConverterOverlay.js` | AngularJS controllers |
| `wwwroot/Views/TypeConverter.html`, `TypeConverterOverlay.html`, `Separator.html` | AngularJS views |
| `wwwroot/Styles/Styles.less` + `Styles.css`, `compilerconfig.json` | Styling for the old views |
| `Manifests/MediaPickerManifestFilter.cs` | Server-side manifest registration |

And replaced with three hand-written files (plain JS Lit elements, no npm/build step — they import from the `@umbraco-cms/backoffice/*` import map provided by the backoffice):

- **`wwwroot/umbraco-package.json`** — the new package manifest, registering:
  - A `propertyEditorSchema` extension for the existing schema alias `Limbo.Umbraco.MediaPicker`, replicating the settings fields of Umbraco's own `Umbraco.MediaPicker3` schema (filter, multiple, validationLimit, startNodeId, enableLocalFocalPoint, crops, ignoreUserStartNodes) **plus** the package's own `typeConverter` field.
  - A `propertyEditorUi` (`Limbo.PropertyEditorUi.MediaPicker`) — the picker UI users select on a data type.
  - A `propertyEditorUi` (`Limbo.PropertyEditorUi.MediaPickerTypeConverter`) — the settings field UI for picking a type converter.
- **`wwwroot/limbo-media-picker.element.js`** — mirrors Umbraco's own `umb-property-editor-ui-media-picker` element by wrapping the publicly exported `<umb-input-rich-media>` component, registered against our schema instead of `Umbraco.MediaPicker3`.
- **`wwwroot/limbo-media-picker-type-converter.element.js`** — replaces the old dropdown/overlay: a `<uui-select>` populated from the management API (authenticated via `UMB_AUTH_CONTEXT` bearer token). Handles the legacy raw-string value format, shows "not found" and "obsolete" warnings like the old view did.

## Behavioural notes / breaking changes for consumers

- **Data type migration**: Umbraco's automatic v13→v14+ migration assigns an `EditorUiAlias` to existing data types. After upgrading a site, verify each Limbo Media Picker data type uses the **Limbo Media Picker** UI (`Limbo.PropertyEditorUi.MediaPicker`) and re-save it.
- **Stored values are unchanged** — property values and data type configuration (including the selected type converter) deserialize as before, including the legacy comma-separated-UDI property format and the legacy raw-string type-converter config format.
- **`IMediaPickerTypeConverter` implementations compile unchanged** — the interface is the same (minus serialization attributes).
- Custom code calling the old backoffice API route must switch to `GET /umbraco/management/api/v1/limbo/media-picker/converters`.
- Site code serializing the output models (`ImageWithCropsItem` etc.) with Newtonsoft will no longer see the `[JsonProperty]` names — the models now carry System.Text.Json attributes instead.

## Verification performed

- `dotnet build --configuration Release /t:rebuild /t:pack` succeeds with zero warnings on .NET 10 / Umbraco 17 packages.
- NuGet package inspected: the three new frontend files are packed as static web assets served at `/App_Plugins/Limbo.Umbraco.MediaPicker/`.
- `umbraco-package.json` validated as JSON; both JS elements syntax-checked with `node --check`.
- Reviewed with the Umbraco extension reviewer agent (verified against the Umbraco 17.5.3 client source). Fixes applied from the review:
  - Removed the redeclaration of `value` in `limbo-media-picker.element.js` — `UmbFormControlMixin` already declares it as a reactive property, and redeclaring it shadows the mixin's accessor.
  - Added `credentials` to the management API fetch in `limbo-media-picker-type-converter.element.js` so it also works with cookie-based backoffice authentication.
  - Added an explicit `[ApiVersion("1.0")]` to `MediaPickerController` (matches the documented Management API controller pattern).
  - Replaced HTML entities in `umbraco-package.json` descriptions with proper markdown/UFM (descriptions are rendered as markdown, not HTML, in v17).


## Notes from claude after work was done
TLDR of changes:
- csproj: net10.0, v17.0.0, deps [17.0.0,17.9.9) as requested — dropped Web.BackOffice/Web.Website, added Infrastructure + Api.Management
- C#: v17 base ctors (no IEditorConfigurationParser; IPublishedMediaCache replaces snapshot accessor), key-only [ConfigurationField], Newtonsoft→System.Text.Json everywhere, controller rewritten as Management API (GET /umbraco/management/api/v1/limbo/media-picker/converters), manifest filter deleted. Keyed IPropertyIndexValueFactory + converter collection + legacy value formats all preserved
- Frontend: AngularJS views/controllers/LESS deleted; replaced by umbraco-package.json (propertyEditorSchema mirroring core MediaPicker3 settings + typeConverter field, 2 propertyEditorUis) and two plain-JS Lit elements — a wrapper around core's <umb-input-rich-media> and a type-converter dropdown hitting the management API. No npm build step needed
- Validation: build + pack green, 0 warnings; nupkg contains the frontend as static web assets; extension-reviewer agent (checked against 17.5.3 source) found no High issues and auto-fixed 4 (mixin value shadowing, fetch credentials, [ApiVersion("1.0")], UFM descriptions)
- Docs: documentation/upgrade-to-umbraco-17.md recap; README + CLAUDE.md bumped to v17

Heads-up for site upgrades: existing data types must be re-pointed at the "Limbo Media Picker" UI and re-saved after Umbraco's v13→v17 migration; site code calling the old /umbraco/backoffice/Limbo/... route or Newtonsoft-serializing the item models needs updating. Untested in a running backoffice — worth a smoke test in a v17 site before release.

