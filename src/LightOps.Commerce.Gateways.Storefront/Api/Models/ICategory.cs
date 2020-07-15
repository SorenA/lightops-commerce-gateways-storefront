namespace LightOps.Commerce.Gateways.Storefront.Api.Models
{
    public interface ICategory
    {
        string Id { get; set; }
        string Handle { get; set; }
        string Url { get; set; }

        string ParentId { get; set; }

        string Title { get; set; }
        string Description { get; set; }

        string SeoTitle { get; set; }
        string SeoDescription { get; set; }

        string PrimaryImage { get; set; }
    }
}