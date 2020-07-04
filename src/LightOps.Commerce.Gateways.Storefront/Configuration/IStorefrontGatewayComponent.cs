namespace LightOps.Commerce.Gateways.Storefront.Configuration
{
    public interface IStorefrontGatewayComponent
    {
        IStorefrontGatewayComponent UseContentPages(string grpcEndpoint);
    }
}