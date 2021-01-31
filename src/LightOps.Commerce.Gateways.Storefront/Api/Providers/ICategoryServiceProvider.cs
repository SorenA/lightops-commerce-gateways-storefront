namespace LightOps.Commerce.Gateways.Storefront.Api.Providers
{
    public interface ICategoryServiceProvider
    {
        bool IsEnabled { get; }
        string GrpcEndpoint { get; }
    }
}