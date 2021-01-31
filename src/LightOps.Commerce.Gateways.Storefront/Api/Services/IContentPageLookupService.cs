using System.Collections.Generic;
using System.Threading.Tasks;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Api.Services
{
    public interface IContentPageLookupService
    {
        Task<IDictionary<string, ContentPage>> LookupByHandleAsync(IEnumerable<string> handles);
        Task<IDictionary<string, ContentPage>> LookupByIdAsync(IEnumerable<string> ids);
    }
}