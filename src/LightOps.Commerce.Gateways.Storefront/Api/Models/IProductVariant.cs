using NodaMoney;

namespace LightOps.Commerce.Gateways.Storefront.Api.Models
{
    public interface IProductVariant
    {
        string Id { get; set; }

        string ProductId { get; set; }

        string Title { get; set; }
        string Sku { get; set; }

        Money Price { get; set; }
    }
}