using System.Collections.Generic;

namespace LightOps.Commerce.Gateways.Storefront.Api.Models
{
    public interface ISubNavigation
    {
        /// <summary>
        /// The header link of the sub-navigation
        /// </summary>
        INavigationLink Header { get; set; }

        /// <summary>
        /// The links in the sub-navigation
        /// </summary>
        IList<INavigationLink> Links { get; set; }

        /// <summary>
        /// The embedded sub-navigations
        /// </summary>
        IList<ISubNavigation> SubNavigations { get; set; }
    }
}