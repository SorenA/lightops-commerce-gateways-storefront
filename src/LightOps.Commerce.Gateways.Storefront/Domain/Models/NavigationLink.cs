using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Models
{
    public class NavigationLink : INavigationLink
    {
        public string Title { get; set; }
        public string Url { get; set; }
        public string Target { get; set; }
    }
}