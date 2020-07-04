namespace LightOps.Commerce.Gateways.Storefront.Api.Models
{
    public interface INavigationLink
    {
        string Title { get; set; }
        string Url { get; set; }
        string Target { get; set; }
    }
}