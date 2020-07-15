namespace LightOps.Commerce.Gateways.Storefront.Api.Providers
{
    public interface ICategoryEndpointProvider
    {
        bool IsEnabled { get; }
        string GrpcEndpoint { get; }
    }
}