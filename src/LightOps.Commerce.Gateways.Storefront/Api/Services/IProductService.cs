using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Api.Services
{
    public interface IProductService
    {
        Task<IProduct> GetByIdAsync(string id);
        Task<IProduct> GetByHandleAsync(string handle);

        Task<IList<IProduct>> GetByIdAsync(IList<string> ids);
        Task<IList<IProduct>> GetByHandleAsync(IList<string> handles);

        Task<IList<IProduct>> GetByCategoryIdAsync(string categoryId);
        Task<IList<IProduct>> GetBySearchAsync(string searchTerm);

        Task<ILookup<string, IProduct>> LookupByIdAsync(IEnumerable<string> ids);
        Task<ILookup<string, IProduct>> LookupByHandleAsync(IEnumerable<string> handles);
    }
}