namespace LightOps.Commerce.Gateways.Storefront.Api.Providers
{
    public interface IMetaFieldEndpointProvider
    {
        bool IsEnabled { get; }
        string GrpcEndpoint { get; }
    }
}