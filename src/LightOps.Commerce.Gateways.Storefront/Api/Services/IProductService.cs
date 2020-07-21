using System.Collections.Generic;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Api.Services
{
    public interface IProductService
    {
        Task<IProduct> GetByIdAsync(string id);
        Task<IList<IProduct>> GetByIdAsync(IList<string> ids);

        Task<IProduct> GetByHandleAsync(string handle);
        Task<IList<IProduct>> GetByHandleAsync(IList<string> handles);

        Task<IList<IProduct>> GetByCategoryIdAsync(string categoryId);
        Task<IDictionary<string, IList<IProduct>>> GetByCategoryIdAsync(IList<string> categoryIds);

        Task<IList<IProduct>> GetBySearchAsync(string searchTerm);
    }
}