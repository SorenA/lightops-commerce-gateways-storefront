using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Api.Services
{
    public interface INavigationLookupService
    {
        Task<IDictionary<string, INavigation>> LookupByHandleAsync(IEnumerable<string> handles);
        Task<IDictionary<string, INavigation>> LookupByIdAsync(IEnumerable<string> ids);
    }
}