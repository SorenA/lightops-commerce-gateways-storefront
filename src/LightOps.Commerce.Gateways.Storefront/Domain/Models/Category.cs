using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Models
{
    public class Category : ICategory
    {
        public string Id { get; set; }
        public string Handle { get; set; }
        public string Url { get; set; }

        public string ParentId { get; set; }

        public string Title { get; set; }
        public string Description { get; set; }

        public string SeoTitle { get; set; }
        public string SeoDescription { get; set; }

        public string PrimaryImage { get; set; }
    }
}