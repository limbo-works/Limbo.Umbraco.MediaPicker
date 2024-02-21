using Umbraco.Cms.Core.Composing;

namespace Limbo.Umbraco.MediaPicker.Converters;

internal sealed class ImageWithCropsTypeConverterCollectionBuilder : LazyCollectionBuilderBase<ImageWithCropsTypeConverterCollectionBuilder, ImageWithCropsTypeConverterCollection, IImageWithCropsTypeConverter> {

    protected override ImageWithCropsTypeConverterCollectionBuilder This => this;

}