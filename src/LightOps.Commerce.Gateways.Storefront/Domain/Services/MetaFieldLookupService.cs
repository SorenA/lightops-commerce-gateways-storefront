using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Services;

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

        public async Task<IDictionary<string, IMetaField>> LookupByIdAsync(IEnumerable<string> ids)
        {
            var result = await _metaFieldService.GetByIdAsync(ids.ToList());
            return result.ToDictionary(x => x.Id);
        }

        public async Task<IDictionary<string, IList<IMetaField>>> LookupByParentIdsAsync(IEnumerable<string> parentIds)
        {
            return await _metaFieldService.GetByParentIdsAsync(parentIds.ToList());
        }
    }
}