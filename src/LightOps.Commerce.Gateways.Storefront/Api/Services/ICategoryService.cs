using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Api.Services
{
    public interface ICategoryService
    {
        Task<ICategory> GetByIdAsync(string id);
        Task<ICategory> GetByHandleAsync(string handle);

        Task<IList<ICategory>> GetByIdAsync(IList<string> ids);
        Task<IList<ICategory>> GetByHandleAsync(IList<string> handles);

        Task<IList<ICategory>> GetByRootAsync();
        Task<IList<ICategory>> GetByParentIdAsync(string parentId);
        Task<IList<ICategory>> GetBySearchAsync(string searchTerm);

        Task<ILookup<string, ICategory>> LookupByIdAsync(IEnumerable<string> ids);
        Task<ILookup<string, ICategory>> LookupByHandleAsync(IEnumerable<string> handles);
    }
}