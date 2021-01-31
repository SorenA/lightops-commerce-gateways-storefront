namespace LightOps.Commerce.Gateways.Storefront.Api.Providers
{
    public interface IProductServiceProvider
    {
        bool IsEnabled { get; }
        string GrpcEndpoint { get; }
    }
}