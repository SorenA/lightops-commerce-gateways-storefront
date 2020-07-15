namespace LightOps.Commerce.Gateways.Storefront.Configuration
{
    public interface IStorefrontGatewayComponent
    {
        IStorefrontGatewayComponent UseContentPages(string grpcEndpoint);
        IStorefrontGatewayComponent UseNavigations(string grpcEndpoint);
        IStorefrontGatewayComponent UseMetaFields(string grpcEndpoint);
        IStorefrontGatewayComponent UseCategories(string grpcEndpoint);
        IStorefrontGatewayComponent UseProducts(string grpcEndpoint);
    }
}