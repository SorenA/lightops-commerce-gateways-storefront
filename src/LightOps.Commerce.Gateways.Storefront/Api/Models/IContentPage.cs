namespace LightOps.Commerce.Gateways.Storefront.Api.Models
{
    public interface IContentPage
    {
        public string Id { get; set; }
        public string Handle { get; set; }
        public string Url { get; set; }

        string ParentId { get; set; }

        string Title { get; set; }
        string Type { get; set; }
        string Description { get; set; }

        string SeoTitle { get; set; }
        string SeoDescription { get; set; }

        string PrimaryImage { get; set; }
    }
}