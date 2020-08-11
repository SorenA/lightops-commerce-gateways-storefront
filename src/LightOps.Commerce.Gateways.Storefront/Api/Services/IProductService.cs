using System.Collections.Generic;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Enums;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.QueryResults;

namespace LightOps.Commerce.Gateways.Storefront.Api.Services
{
    public interface IProductService
    {
        /// <summary>
        /// Gets a list of products by handle
        /// </summary>
        /// <param name="handles">The handles of the products</param>
        /// <returns>List of products, if any</returns>
        Task<IList<IProduct>> GetByHandleAsync(IList<string> handles);

        /// <summary>
        /// Gets a list of products by ids
        /// </summary>
        /// <param name="ids">The ids of the products</param>
        /// <returns>List of products, if any</returns>
        Task<IList<IProduct>> GetByIdAsync(IList<string> ids);

        /// <summary>
        /// Gets a list of paginated products by search
        /// </summary>
        /// <param name="searchTerm">The term to search for</param>
        /// <param name="categoryId">Search only in children with a specific category id, if any specified</param>
        /// <param name="pageCursor">The page cursor to use</param>
        /// <param name="pageSize">The page size to use</param>
        /// <param name="sortKey">Sort the underlying list by the given key</param>
        /// <param name="reverse">Whether to reverse the order of the underlying list</param>
        /// <returns>Search result with products matching search</returns>
        Task<SearchResult<IProduct>> GetBySearchAsync(string searchTerm,
                                                      string categoryId,
                                                      string pageCursor,
                                                      int pageSize,
                                                      ProductSortKey sortKey,
                                                      bool reverse);
    }
}