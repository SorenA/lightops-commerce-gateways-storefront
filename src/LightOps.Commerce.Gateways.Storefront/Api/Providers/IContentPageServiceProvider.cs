namespace LightOps.Commerce.Gateways.Storefront.Api.Providers
{
    public interface IContentPageServiceProvider
    {
        bool IsEnabled { get; }
        string GrpcEndpoint { get; }
    }
}