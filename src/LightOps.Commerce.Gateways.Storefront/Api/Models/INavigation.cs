using System;
using System.Collections.Generic;

namespace LightOps.Commerce.Gateways.Storefront.Api.Models
{
    public interface INavigation
    {
        /// <summary>
        /// Globally unique identifier, eg: gid://Navigation/1000
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Globally unique identifier of parent, 'gid://' if none
        /// </summary>
        string ParentId { get; set; }

        /// <summary>
        /// A human-friendly unique string for the navigation
        /// </summary>
        string Handle { get; set; }

        /// <summary>
        /// The type of the navigation
        /// </summary>
        string Type { get; set; }

        /// <summary>
        /// The timestamp of navigation creation
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// The timestamp of the latest navigation update
        /// </summary>
        DateTime UpdatedAt { get; set; }

        /// <summary>
        /// The header link of the navigation
        /// </summary>
        INavigationLink Header { get; set; }

        /// <summary>
        /// The links in the navigation
        /// </summary>
        IList<INavigationLink> Links { get; set; }

        /// <summary>
        /// The embedded sub-navigations
        /// </summary>
        IList<ISubNavigation> SubNavigations { get; set; }
    }
}