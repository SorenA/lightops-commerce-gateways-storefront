using System;
using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Models
{
    public class ContentPage : IContentPage
    {
        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Handle { get; set; }
        public string Title { get; set; }
        public string Url { get; set; }
        public string Type { get; set; }
        public string Summary { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public IImage PrimaryImage { get; set; }
    }
}