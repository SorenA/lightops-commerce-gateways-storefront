using System.Collections.Generic;

namespace LightOps.Commerce.Gateways.Storefront.Api.Models
{
    public interface IProduct
    {
        string Id { get; set; }
        string Handle { get; set; }
        string Url { get; set; }

        string Title { get; set; }
        string Type { get; set; }
        string Description { get; set; }

        string SeoTitle { get; set; }
        string SeoDescription { get; set; }

        string PrimaryCategoryId { get; set; }
        IList<string> CategoryIds { get; set; }

        IList<IProductVariant> Variants { get; set; }

        string PrimaryImage { get; set; }
        IList<string> Images { get; set; }
    }
}