using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Services;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Services
{
    public class CategoryLookupService : ICategoryLookupService
    {
        private readonly ICategoryService _categoryService;

        public CategoryLookupService(
            ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }
        
        public async Task<IDictionary<string, ICategory>> LookupByIdAsync(IEnumerable<string> ids)
        {
            var result = await _categoryService.GetByIdAsync(ids.ToList());
            return result.ToDictionary(x => x.Id);
        }

        public async Task<IDictionary<string, ICategory>> LookupByHandleAsync(IEnumerable<string> handles)
        {
            var result = await _categoryService.GetByHandleAsync(handles.ToList());
            return result.ToDictionary(x => x.Id);
        }

        public async Task<IDictionary<string, IList<ICategory>>> LookupByParentIdAsync(IEnumerable<string> parentIds)
        {
            var result = await _categoryService.GetByParentIdAsync(parentIds.ToList());
            return result
                .GroupBy(x => x.ParentId)
                .ToDictionary(
                    x => x.Key,
                    x => (IList<ICategory>)x.ToList());
        }
    }
}