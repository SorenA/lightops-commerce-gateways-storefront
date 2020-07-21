using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Api.Services
{
    public interface IProductLookupService
    {
        Task<IDictionary<string, IProduct>> LookupByIdAsync(IEnumerable<string> ids);
        Task<IDictionary<string, IProduct>> LookupByHandleAsync(IEnumerable<string> handles);
        Task<IDictionary<string, IList<IProduct>>> LookupByCategoryIdAsync(IEnumerable<string> categoryIds);
    }
}