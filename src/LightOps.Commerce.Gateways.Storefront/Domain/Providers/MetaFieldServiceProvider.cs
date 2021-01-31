using LightOps.Commerce.Gateways.Storefront.Api.Providers;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Providers
{
    public class MetaFieldServiceProvider : IMetaFieldServiceProvider
    {
        public bool IsEnabled { get; internal set; }
        public string GrpcEndpoint { get; internal set; }
    }
}