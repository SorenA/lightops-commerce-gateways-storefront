using System;
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
            Images = new List<IImage>();
        }

        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Handle { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public string PrimaryCategoryId { get; set; }
        public IList<string> CategoryIds { get; set; }
        public IList<IProductVariant> Variants { get; set; }
        public IList<IImage> Images { get; set; }
    }
}