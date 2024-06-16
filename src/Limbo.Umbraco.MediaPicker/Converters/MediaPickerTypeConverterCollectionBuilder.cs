using Umbraco.Cms.Core.Composing;

namespace Limbo.Umbraco.MediaPicker.Converters;

internal sealed class MediaPickerTypeConverterCollectionBuilder : LazyCollectionBuilderBase<MediaPickerTypeConverterCollectionBuilder, MediaPickerTypeConverterCollection, IMediaPickerTypeConverter> {

    protected override MediaPickerTypeConverterCollectionBuilder This => this;

}