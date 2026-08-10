# Limbo Media Picker

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](https://github.com/limbo-works/Limbo.Umbraco.MediaPicker/blob/v17/main/LICENSE.md)
[![NuGet](https://img.shields.io/nuget/vpre/Limbo.Umbraco.MediaPicker.svg)](https://www.nuget.org/packages/Limbo.Umbraco.MediaPicker)
[![NuGet](https://img.shields.io/nuget/dt/Limbo.Umbraco.MediaPicker.svg)](https://www.nuget.org/packages/Limbo.Umbraco.MediaPicker)
[![Our Umbraco](https://img.shields.io/badge/our-umbraco-%233544B1)](https://our.umbraco.com/packages/backoffice-extensions/limbo-media-picker/)
[![Umbraco Marketplace](https://img.shields.io/badge/umbraco-marketplace-%233544B1)](https://marketplace.umbraco.com/package/limbo.umbraco.mediapicker)

**Limbo.Umbraco.MediaPicker** is a package for Umbraco with property editors extending Umbraco's default media picker.

<table>
  <tr>
    <td><strong>License:</strong></td>
    <td><a href="https://github.com/limbo-works/Limbo.Umbraco.MediaPicker/blob/v17/main/LICENSE.md"><strong>MIT License</strong></a></td>
  </tr>
  <tr>
    <td><strong>Umbraco:</strong></td>
    <td>Umbraco 17</td>
  </tr>
  <tr>
    <td><strong>Target Framework:</strong></td>
    <td>.NET 10</td>
  </tr>
</table>





<br /><br />

## Installation

### Umbraco 17

The latest version of the package targets Umbraco 17 and is available via [**NuGet**][NuGetPackage]. To install the package, you can use either .NET CLI:

```
dotnet add package Limbo.Umbraco.MediaPicker --version 17.0.0
```

or the NuGet Package Manager:

```
Install-Package Limbo.Umbraco.MediaPicker -Version 17.0.0
```

### Umbraco 13

See the [**`v13/main`**](https://github.com/limbo-works/Limbo.Umbraco.MediaPicker/tree/v13/main) branch.

### Umbraco 10, 11 and 12

See the [**`v1.x`**](https://github.com/limbo-works/Limbo.Umbraco.MediaPicker/tree/v1/main) branch.

### Umbraco 7 and 8

See our older [**Skybrud.ImagePicker**](https://github.com/skybrud/Skybrud.ImagePicker) package.



<br /><br />

## Features

- Allows selecting a *type converter* on the data type, in which case the type converter will be used for converting the selected items from Umbraco's `MediaWithCrops` to a desired type.



<br /><br />

## Documentation

- [See the documentation at **packages.limbo.works**](https://packages.limbo.works/limbo.umbraco.mediapicker/docs/v17/)
- [Upgrade recap: Umbraco 13 → 17](./documentation/upgrade-to-umbraco-17.md)







[NuGetPackage]: https://www.nuget.org/packages/Limbo.Umbraco.MediaPicker
[UmbracoPackage]: https://our.umbraco.com/packages/backoffice-extensions/limbo-image-picker/
[GitHubRelease]: https://github.com/limbo-works/Limbo.Umbraco.MediaPicker/releases



