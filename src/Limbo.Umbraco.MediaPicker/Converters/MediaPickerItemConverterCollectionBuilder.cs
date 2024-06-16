using Umbraco.Cms.Core.Composing;

namespace Limbo.Umbraco.MediaPicker.Converters;

internal sealed class MediaPickerItemConverterCollectionBuilder : LazyCollectionBuilderBase<MediaPickerItemConverterCollectionBuilder, MediaPickerItemConverterCollection, IMediaPickerItemConverter> {

    protected override MediaPickerItemConverterCollectionBuilder This => this;

}