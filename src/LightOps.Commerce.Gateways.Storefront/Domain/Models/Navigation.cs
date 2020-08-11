﻿using System;
using System.Collections.Generic;
using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Models
{
    public class Navigation : INavigation
    {
        public Navigation()
        {
            Links = new List<INavigationLink>();
            SubNavigations = new List<ISubNavigation>();
        }

        public string Id { get; set; }
        public string ParentId { get; set; }
        public string Handle { get; set; }
        public string Type { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public INavigationLink Header { get; set; }
        public IList<INavigationLink> Links { get; set; }
        public IList<ISubNavigation> SubNavigations { get; set; }
    }
}