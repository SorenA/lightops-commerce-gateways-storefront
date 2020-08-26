using LightOps.Commerce.Gateways.Storefront.Api.Providers;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Providers
{
    public class ImageCdnProvider : IImageCdnProvider
    {
        public bool IsEnabled { get; internal set; }
        public string CdnHost { get; internal set; }
    }
}