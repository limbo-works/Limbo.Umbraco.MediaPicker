# CLAUDE.md

This file provides guidance to Claude Code (claude.ai/code) when working with code in this repository.

## What this is

NuGet package `Limbo.Umbraco.MediaPicker` — an Umbraco 17 (.NET 10) property editor that extends Umbraco's built-in MediaPicker3 with a configurable "type converter" that transforms picked `MediaWithCrops` items into custom types. No test project exists. Upgraded from Umbraco 13 on the `v17/dev` branch — see `documentation/upgrade-to-umbraco-17.md` for the full recap.

## Commands

```sh
# Build
dotnet build src/Limbo.Umbraco.MediaPicker

# Release build + NuGet pack into ./releases/nuget (same as release.bat)
dotnet build src/Limbo.Umbraco.MediaPicker --configuration Release /t:rebuild /t:pack -p:PackageOutputPath=../../releases/nuget
```

`debug.bat` packs a Debug build (timestamped version suffix) to `c:\nuget\Umbraco17` — Windows-only, irrelevant on macOS.

The frontend is hand-written plain-JS Lit modules in `wwwroot/` — no npm/vite build step. They import from the `@umbraco-cms/backoffice/*` import map provided by the backoffice at runtime.

## Branches

Version branches: `v13/main` is the default/PR target for Umbraco 13 (previous LTS), `v1/main` is Umbraco 10–12 legacy, `v17/dev` is the Umbraco 17 line. The `docs` branch holds documentation.

## Architecture

Everything lives in the single project `src/Limbo.Umbraco.MediaPicker/`. A property editor in Umbraco 14+ is split in two halves: a C# **schema** (data storage/conversion) and a client-side **UI** (Lit elements + `umbraco-package.json`).

Server side (C#):

- `PropertyEditors/LimboMediaPickerEditor` (schema alias `Limbo.Umbraco.MediaPicker`) extends `MediaPicker3PropertyEditor`. It injects a keyed `IPropertyIndexValueFactory` (keyed on `MediaPickerPackage.Alias`) so consumers can override search indexing via `AddLimboMediaPickerPropertyIndexValueFactory<T>()` (default `NoopPropertyIndexValueFactory`, registered in the composer).
- `Models/LimboMediaPickerConfiguration` extends `MediaPicker3Configuration` with the `typeConverter` field; `[ConfigurationField]` is key-only in v17 — all field labels/UIs are declared client-side.
- `PropertyEditors/ValueConverters/LimboMediaPickerValueConverter` extends `MediaPickerWithCropsValueConverter`; after base conversion it looks up the data type's selected converter in `MediaPickerTypeConverterCollection` and runs it. `GetPropertyValueType` delegates to the converter's `GetType()` so models builder types are correct.
- Converter system: implement `Converters/IMediaPickerTypeConverter` (or `MediaPickerItemConverterBase`) anywhere — auto-discovered via `TypeLoader` into `MediaPickerTypeConverterCollection` by `Composers/MediaPickerComposer`. Converter identity is an alias derived from the CLR type (`MediaPickerUtils.GetTypeAlias`).
- `Controllers/Api/MediaPickerController` is a Management API controller (`GET /umbraco/management/api/v1/limbo/media-picker/converters`) feeding the type-converter dropdown.
- JSON is System.Text.Json throughout (Umbraco 14+ convention); `Json/MediaPickerTypeConverterJsonConverter` also accepts the legacy raw-string config format, and `Json/MediaWithCropsDeserializer` accepts the legacy comma-separated-UDI property format.

Client side (`wwwroot/`, served at `/App_Plugins/Limbo.Umbraco.MediaPicker/` via static web assets):

- `umbraco-package.json` registers a `propertyEditorSchema` for `Limbo.Umbraco.MediaPicker` (replicating core's `Umbraco.MediaPicker3` settings fields + `typeConverter`) and two `propertyEditorUi` extensions.
- `limbo-media-picker.element.js` mirrors core's media picker UI by wrapping `<umb-input-rich-media>` (public export of `@umbraco-cms/backoffice/media`).
- `limbo-media-picker-type-converter.element.js` is the settings dropdown, fetching converters from the management API with a bearer token from `UMB_AUTH_CONTEXT`.

## Conventions

- C# files use `#region` blocks (Constants/Constructors/Member methods) and same-line braces; follow the existing style.
- Public members carry XML doc comments (the build generates a documentation file); some files opt out with `#pragma warning disable CS1591`.
- Umbraco package references pin to `[17.0.0,17.9.9)` with `NoWarn="NU1902"`; `NuGetAuditMode` is `direct` because Umbraco's transitive deps carry advisories we can't fix.
