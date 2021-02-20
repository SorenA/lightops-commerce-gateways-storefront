using System.Collections.Generic;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.QueryResults;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Api.Services
{
    public interface IProductService
    {
        /// <summary>
        /// Gets a list of products by handle
        /// </summary>
        /// <param name="handles">The handles of the products</param>
        /// <param name="languageCode">The language code to match handles in<br/>ISO 639 2-letter language code matched with ISO 3166 2-letter country code, eg. en-US, da-DK</param>
        /// <returns>List of products, if any</returns>
        Task<IList<Product>> GetByHandleAsync(IList<string> handles,
                                              string languageCode);

        /// <summary>
        /// Gets a list of products by ids
        /// </summary>
        /// <param name="ids">The ids of the products</param>
        /// <returns>List of products, if any</returns>
        Task<IList<Product>> GetByIdAsync(IList<string> ids);

        /// <summary>
        /// Gets a list of paginated products by search
        /// </summary>
        /// <param name="searchTerm">The term to search for</param>
        /// <param name="languageCode">Search only in localized strings with a specific language code, if any specified<br/>ISO 639 2-letter language code matched with ISO 3166 2-letter country code, eg. en-US, da-DK</param>
        /// <param name="categoryId">Search only in children with a specific category id, if any specified</param>
        /// <param name="pageCursor">The page cursor to use</param>
        /// <param name="pageSize">The page size to use</param>
        /// <param name="sortKey">Sort the underlying list by the given key</param>
        /// <param name="reverse">Whether to reverse the order of the underlying list</param>
        /// <param name="currencyCode">The currency code to use for sorting if sorting by currency<br/>ISO 4217 3-letter currency code</param>
        /// <returns>Search result with products matching search</returns>
        Task<SearchResult<Product>> GetBySearchAsync(string searchTerm,
                                                     string languageCode,
                                                     string categoryId,
                                                     string pageCursor,
                                                     int pageSize,
                                                     ProductSortKey sortKey,
                                                     bool reverse,
                                                     string currencyCode);
    }
}