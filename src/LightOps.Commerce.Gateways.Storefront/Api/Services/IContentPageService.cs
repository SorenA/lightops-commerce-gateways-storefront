using System.Collections.Generic;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.QueryResults;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Api.Services
{
    public interface IContentPageService
    {
        /// <summary>
        /// Gets a list of content pages by handle
        /// </summary>
        /// <param name="handles">The handles of the content pages</param>
        /// <returns>List of content pages, if any</returns>
        Task<IList<ContentPage>> GetByHandleAsync(IList<string> handles);

        /// <summary>
        /// Gets a list of content pages by ids
        /// </summary>
        /// <param name="ids">The ids of the content pages</param>
        /// <returns>List of content pages, if any</returns>
        Task<IList<ContentPage>> GetByIdAsync(IList<string> ids);

        /// <summary>
        /// Gets a list of paginated content pages by search
        /// </summary>
        /// <param name="searchTerm">The term to search for</param>
        /// <param name="languageCode">Search only in localized strings with a specific language code, if any specified<br/>ISO 639 2-letter language code matched with ISO 3166 2-letter country code, eg. en-US, da-DK</param>
        /// <param name="parentId">Search only in children with a specific parent id, if any specified. For no parent: 'gid://'</param>
        /// <param name="pageCursor">The page cursor to use</param>
        /// <param name="pageSize">The page size to use</param>
        /// <param name="sortKey">Sort the underlying list by the given key</param>
        /// <param name="reverse">Whether to reverse the order of the underlying list</param>
        /// <returns>Search result with content pages matching search</returns>
        Task<SearchResult<ContentPage>> GetBySearchAsync(string searchTerm,
                                                         string languageCode,
                                                         string parentId,
                                                         string pageCursor,
                                                         int pageSize,
                                                         ContentPageSortKey sortKey,
                                                         bool reverse);
    }
}
