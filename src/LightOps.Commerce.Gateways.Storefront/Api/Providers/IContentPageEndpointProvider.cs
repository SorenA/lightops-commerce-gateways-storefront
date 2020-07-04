namespace LightOps.Commerce.Gateways.Storefront.Api.Providers
{
    public interface IContentPageEndpointProvider
    {
        bool IsEnabled { get; }
        string GrpcEndpoint { get; }
    }
}