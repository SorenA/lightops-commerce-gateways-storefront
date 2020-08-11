using System.Collections.Generic;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Api.Services
{
    public interface IMetaFieldLookupService
    {
        Task<IDictionary<string, IMetaField>> LookupByIdAsync(IEnumerable<string> ids);
        Task<IDictionary<string, IList<IMetaField>>> LookupByParentIdsAsync(IEnumerable<string> parentIds);
    }
}