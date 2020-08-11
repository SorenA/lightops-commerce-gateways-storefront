using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Api.Services
{
    public interface ICategoryLookupService
    {
        Task<IDictionary<string, ICategory>> LookupByHandleAsync(IEnumerable<string> handles);
        Task<IDictionary<string, ICategory>> LookupByIdAsync(IEnumerable<string> ids);
    }
}