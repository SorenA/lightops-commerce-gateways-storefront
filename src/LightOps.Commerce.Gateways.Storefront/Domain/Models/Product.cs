using System.Collections.Generic;
using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Models
{
    public class Product : IProduct
    {
        public Product()
        {
            CategoryIds = new List<string>();
            Variants = new List<IProductVariant>();
            Images = new List<string>();
        }

        public string Id { get; set; }
        public string Handle { get; set; }
        public string Url { get; set; }

        public string Title { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }

        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }

        public string PrimaryCategoryId { get; set; }
        public IList<string> CategoryIds { get; set; }

        public IList<IProductVariant> Variants { get; set; }

        public string PrimaryImage { get; set; }
        public IList<string> Images { get; set; }
    }
}