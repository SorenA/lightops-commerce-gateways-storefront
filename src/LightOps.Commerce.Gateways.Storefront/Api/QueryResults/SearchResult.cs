using System.Collections.Generic;

namespace LightOps.Commerce.Gateways.Storefront.Api.QueryResults
{
    public class SearchResult<T>
    {
        public SearchResult()
        {
            Results = new List<T>();
        }

        /// <summary>
        /// The results found, if any
        /// </summary>
        public IList<T> Results { get; set; }

        /// <summary>
        /// The cursor of the next page
        /// </summary>
        public string NextPageCursor { get; set; }

        /// <summary>
        /// Whether another page can be fetched
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        /// The total amount of results available
        /// </summary>
        public int TotalResults { get; set; }
    }
}