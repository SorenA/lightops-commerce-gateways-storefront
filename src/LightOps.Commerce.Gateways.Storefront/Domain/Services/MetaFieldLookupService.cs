using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Services
{
    public class MetaFieldLookupService : IMetaFieldLookupService
    {
        private readonly IMetaFieldService _metaFieldService;

        public MetaFieldLookupService(
            IMetaFieldService metaFieldService)
        {
            _metaFieldService = metaFieldService;
        }

        public async Task<IDictionary<string, MetaField>> LookupByIdAsync(IEnumerable<string> ids)
        {
            var result = await _metaFieldService.GetByIdAsync(ids.ToList());
            return result.ToDictionary(x => x.Id);
        }

        public async Task<IDictionary<string, IList<MetaField>>> LookupByParentIdsAsync(IEnumerable<string> parentIds)
        {
            return await _metaFieldService.GetByParentIdsAsync(parentIds.ToList());
        }
    }
}