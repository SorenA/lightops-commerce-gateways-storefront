using System.Collections.Generic;
using System.Threading.Tasks;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Api.Services
{
    public interface IProductLookupService
    {
        Task<IDictionary<string, Product>> LookupByHandleAsync(IEnumerable<string> handles);
        Task<IDictionary<string, Product>> LookupByIdAsync(IEnumerable<string> ids);
    }
}