using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Proto.Types;

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

        public async Task<IDictionary<string, Category>> LookupByHandleAsync(IEnumerable<string> handles, string languageCode)
        {
            var result = await _categoryService.GetByHandleAsync(handles.ToList(), languageCode);
            return result.ToDictionary(x => x.Handles
                .FirstOrDefault(ls => ls.LanguageCode == languageCode)?.Value);
        }

        public async Task<IDictionary<string, Category>> LookupByIdAsync(IEnumerable<string> ids)
        {
            var result = await _categoryService.GetByIdAsync(ids.ToList());
            return result.ToDictionary(x => x.Id);
        }
    }
}