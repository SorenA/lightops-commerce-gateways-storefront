using System.Collections.Generic;
using System.Threading.Tasks;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Api.Services
{
    public interface INavigationLookupService
    {
        Task<IDictionary<string, Navigation>> LookupByHandleAsync(IEnumerable<string> handles, string languageCode);
        Task<IDictionary<string, Navigation>> LookupByIdAsync(IEnumerable<string> ids);
    }
}