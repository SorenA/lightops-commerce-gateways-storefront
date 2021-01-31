using System.Collections.Generic;
using System.Threading.Tasks;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Api.Services
{
    public interface IMetaFieldLookupService
    {
        Task<IDictionary<string, MetaField>> LookupByIdAsync(IEnumerable<string> ids);
        Task<IDictionary<string, IList<MetaField>>> LookupByParentIdsAsync(IEnumerable<string> parentIds);
    }
}