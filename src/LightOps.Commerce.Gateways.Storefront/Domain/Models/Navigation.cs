using System.Collections.Generic;
using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Models
{
    public class Navigation : INavigation
    {
        public Navigation()
        {
            Links = new List<INavigationLink>();
            SubNavigations = new List<INavigation>();
        }

        public string Id { get; set; }
        public string Handle { get; set; }

        public string ParentNavigationId { get; set; }

        public INavigationLink Header { get; set; }
        public IList<INavigationLink> Links { get; set; }

        public IList<INavigation> SubNavigations { get; set; }
    }
}