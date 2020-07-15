namespace LightOps.Commerce.Gateways.Storefront.Api.Providers
{
    public interface IProductEndpointProvider
    {
        bool IsEnabled { get; }
        string GrpcEndpoint { get; }
    }
}