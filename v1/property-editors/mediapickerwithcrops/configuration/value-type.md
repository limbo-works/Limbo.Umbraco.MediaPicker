# Value Type

If nothing else is specified, selected images will be returned as instances of <code>Limbo.Umbraco.MediaPicker.Models.MediaWithCropsItem</code>. However if another model is more desired, a custom type can be set via the *Value type* option in the data type configuration.

> **INFO:** The [type converter](./../type-converter/) option takes predence, so the value type will be ignored if a type converter has been selected.

The option lets you pick any .NET type that has a public constructor, and the first parameter is either of the type `MediaWithCropsItem` or `IPublishedContent`. If a constructor has any additional parameters, the package will try resolving their values via dependency injection.

If the second parameter is of type <code>Limbo.Umbraco.MediaPicker.Models.LimboMediaPickerWithCropsConfiguration</code>, the configuration of the data type will also be supplied to the model.

![image](https://user-images.githubusercontent.com/3634580/164420748-0c5bb999-f445-4b26-9a20-23b61bf4a561.png)
