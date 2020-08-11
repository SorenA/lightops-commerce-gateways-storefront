﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Services;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Services
{
    public class NavigationLookupService : INavigationLookupService
    {
        private readonly INavigationService _navigationService;

        public NavigationLookupService(
            INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public async Task<IDictionary<string, INavigation>> LookupByHandleAsync(IEnumerable<string> handles)
        {
            var result = await _navigationService.GetByHandleAsync(handles.ToList());
            return result.ToDictionary(x => x.Handle);
        }

        public async Task<IDictionary<string, INavigation>> LookupByIdAsync(IEnumerable<string> ids)
        {
            var result = await _navigationService.GetByIdAsync(ids.ToList());
            return result.ToDictionary(x => x.Id);
        }
    }
}