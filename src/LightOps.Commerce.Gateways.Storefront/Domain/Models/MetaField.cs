using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Models
{
    public class MetaField : IMetaField
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string Value { get; set; }
    }
}