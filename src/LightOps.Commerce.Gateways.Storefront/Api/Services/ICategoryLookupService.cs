using System.Collections.Generic;
using System.Threading.Tasks;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Api.Services
{
    public interface ICategoryLookupService
    {
        Task<IDictionary<string, Category>> LookupByHandleAsync(IEnumerable<string> handles, string languageCode);
        Task<IDictionary<string, Category>> LookupByIdAsync(IEnumerable<string> ids);
    }
}