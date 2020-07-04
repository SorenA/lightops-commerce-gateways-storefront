using System.Collections.Generic;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Api.Services
{
    public interface INavigationService
    {
        Task<INavigation> GetByIdAsync(string id);
        Task<INavigation> GetByHandleAsync(string handle);

        Task<IList<INavigation>> GetByRootAsync();
        Task<IList<INavigation>> GetByParentIdAsync(string parentId);
    }
}