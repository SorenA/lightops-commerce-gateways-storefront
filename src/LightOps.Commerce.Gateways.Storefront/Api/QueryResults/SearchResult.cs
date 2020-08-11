using System.Collections.Generic;
using System.Linq;
using GraphQL.Types.Relay.DataObjects;

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

        public Connection<T> ToGraphConnection()
        {
            return new Connection<T>
            {
                TotalCount = TotalResults,
                PageInfo = new PageInfo
                {
                    HasNextPage = HasNextPage,
                    EndCursor = NextPageCursor,
                },
                Edges = Results
                    .Select(x => new Edge<T>
                    {
                        Cursor = null,
                        Node = x,
                    })
                    .ToList(),
            };
        }
    }
}