namespace LightOps.Commerce.Gateways.Storefront.Api.Providers
{
    public interface IMetaFieldServiceProvider
    {
        bool IsEnabled { get; }
        string GrpcEndpoint { get; }
    }
}