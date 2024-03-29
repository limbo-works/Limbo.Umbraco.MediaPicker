# Limbo Media Picker

[![GitHub license](https://img.shields.io/badge/license-MIT-blue.svg)](LICENSE.md) [![NuGet](https://img.shields.io/nuget/vpre/Limbo.Umbraco.MediaPicker.svg)](https://www.nuget.org/packages/Limbo.Umbraco.MediaPicker) [![NuGet](https://img.shields.io/nuget/dt/Limbo.Umbraco.MediaPicker.svg)](https://www.nuget.org/packages/Limbo.Umbraco.MediaPicker) [![Our Umbraco](https://img.shields.io/badge/our-umbraco-%233544B1)](https://our.umbraco.com/packages/backoffice-extensions/limbo-media-picker/) [![Umbraco Marketplace](https://img.shields.io/badge/umbraco-marketplace-%233544B1)](https://marketplace.umbraco.com/package/limbo.umbraco.mediapicker)

**Limbo.Umbraco.MediaPicker** is a package for Umbraco 10+ with property editors extending Umbraco's default media pickers.

<table>
  <tr>
    <td><strong>License:</strong></td>
    <td><a href="./LICENSE.md"><strong>MIT License</strong></a></td>
  </tr>
  <tr>
    <td><strong>Umbraco:</strong></td>
    <td>Umbraco 10, 11 and 12</td>
  </tr>
  <tr>
    <td><strong>Target Framework:</strong></td>
    <td>.NET 6</td>
  </tr>
</table>





<br /><br />

## Installation

The package targets Umbraco 10+ and is available via [**NuGet**][NuGetPackage]. To install the package, you can use either .NET CLI:

```
dotnet add package Limbo.Umbraco.MediaPicker --version 1.0.2
```

or the NuGet Package Manager:

```
Install-Package Limbo.Umbraco.MediaPicker -Version 1.0.2
```

For older versions of Umbraco, see our older [**Skybrud.ImagePicker**](https://github.com/skybrud/Skybrud.ImagePicker) package.



<br /><br />

## Features

- Allows selecting a *type converter* on the data type, in which case the type converter will be used for converting the selected items from Umbraco's `MediaWithCrops` to a desired type.

- Allows selecting a *value type* on the data type, in which case this type is used instead of Umbraco's `MediaWithCrops` model.



<br /><br />

## Documentation

- [See the documentation at **packages.limbo.works**](https://packages.limbo.works/limbo.umbraco.mediapicker/docs/v1/)







[NuGetPackage]: https://www.nuget.org/packages/Limbo.Umbraco.MediaPicker
[UmbracoPackage]: https://our.umbraco.com/packages/backoffice-extensions/limbo-image-picker/
[GitHubRelease]: https://github.com/limbo-works/Limbo.Umbraco.MediaPicker/releases



