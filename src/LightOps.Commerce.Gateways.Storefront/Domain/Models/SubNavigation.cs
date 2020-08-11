using System.Collections.Generic;
using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Models
{
    public class SubNavigation : ISubNavigation
    {
        public SubNavigation()
        {
            Links = new List<INavigationLink>();
            SubNavigations = new List<ISubNavigation>();
        }

        public INavigationLink Header { get; set; }
        public IList<INavigationLink> Links { get; set; }
        public IList<ISubNavigation> SubNavigations { get; set; }
    }
}