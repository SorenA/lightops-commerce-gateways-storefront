namespace LightOps.Commerce.Gateways.Storefront.Api.Models
{
    public interface IMetaField
    {
        string Name { get; set; }
        string Type { get; set; }
        string Value { get; set; }
    }
}