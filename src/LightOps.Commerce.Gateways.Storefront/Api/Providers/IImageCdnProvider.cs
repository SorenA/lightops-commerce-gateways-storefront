namespace LightOps.Commerce.Gateways.Storefront.Api.Providers
{
    public interface IImageCdnProvider
    {
        bool IsEnabled { get; }
        string CdnHost { get; }
    }
}