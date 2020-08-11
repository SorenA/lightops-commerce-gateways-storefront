using System.Collections.Generic;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using NodaMoney;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Models
{
    public class ProductVariant : IProductVariant
    {
        public string Id { get; set; }
        public string ProductId { get; set; }
        public string Title { get; set; }
        public string Sku { get; set; }
        public Money UnitPrice { get; set; }
        public IList<IImage> Images { get; set; }
    }
}