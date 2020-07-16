﻿using System.Collections.Generic;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Api.Services
{
    public interface INavigationService
    {
        Task<INavigation> GetByIdAsync(string id);
        Task<INavigation> GetByHandleAsync(string handle);

        Task<IList<INavigation>> GetByIdAsync(IList<string> ids);
        Task<IList<INavigation>> GetByHandleAsync(IList<string> handles);

        Task<IList<INavigation>> GetByRootAsync();
        Task<IList<INavigation>> GetByParentIdAsync(string parentId);
    }
}