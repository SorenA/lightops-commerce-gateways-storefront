using System.Collections.Generic;

namespace LightOps.Commerce.Gateways.Storefront.Api.Models
{
    public interface INavigation
    {
        string Id { get; set; }
        string Handle { get; set; }

        string ParentNavigationId { get; set; }

        INavigationLink Header { get; set; }
        IList<INavigationLink> Links { get; set; }

        IList<INavigation> SubNavigations { get; set; }
    }
}