using System.Collections.Generic;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.QueryResults;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Api.Services
{
    public interface ICategoryService
    {
        /// <summary>
        /// Gets a list of categories by handle
        /// </summary>
        /// <param name="handles">The handles of the categories</param>
        /// <returns>List of categories, if any</returns>
        Task<IList<Category>> GetByHandleAsync(IList<string> handles);

        /// <summary>
        /// Gets a list of categories by ids
        /// </summary>
        /// <param name="ids">The ids of the categories</param>
        /// <returns>List of categories, if any</returns>
        Task<IList<Category>> GetByIdAsync(IList<string> ids);


        /// <summary>
        /// Gets a list of paginated categories by search
        /// </summary>
        /// <param name="searchTerm">The term to search for</param>
        /// <param name="languageCode">Search only in localized strings with a specific language code, if any specified<br/>ISO 639 2-letter language code matched with ISO 3166 2-letter country code, eg. en-US, da-DK</param>
        /// <param name="parentId">Search only in children with a specific parent id, if any specified. For no parent: 'gid://'</param>
        /// <param name="pageCursor">The page cursor to use</param>
        /// <param name="pageSize">The page size to use</param>
        /// <param name="sortKey">Sort the underlying list by the given key</param>
        /// <param name="reverse">Whether to reverse the order of the underlying list</param>
        /// <returns>Search result with categories matching search</returns>
        Task<SearchResult<Category>> GetBySearchAsync(string searchTerm,
                                                      string languageCode,
                                                      string parentId,
                                                      string pageCursor,
                                                      int pageSize,
                                                      CategorySortKey sortKey,
                                                      bool reverse);
    }
}