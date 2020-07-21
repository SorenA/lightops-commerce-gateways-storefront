using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Api.Services
{
    public interface IContentPageLookupService
    {
        Task<IDictionary<string, IContentPage>> LookupByIdAsync(IEnumerable<string> ids);
        Task<IDictionary<string, IContentPage>> LookupByHandleAsync(IEnumerable<string> handles);
        Task<IDictionary<string, IList<IContentPage>>> LookupByParentIdAsync(IEnumerable<string> parentIds);
    }
}