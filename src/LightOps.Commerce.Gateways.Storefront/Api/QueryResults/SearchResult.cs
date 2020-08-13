using System.Collections.Generic;
using System.Linq;
using GraphQL.Types.Relay.DataObjects;

namespace LightOps.Commerce.Gateways.Storefront.Api.QueryResults
{
    public class SearchResult<T>
    {
        public SearchResult()
        {
            Results = new List<CursorNodeResult<T>>();
        }

        /// <summary>
        /// The results found, if any
        /// </summary>
        public IList<CursorNodeResult<T>> Results { get; set; }

        /// <summary>
        /// The cursor of the first result
        /// </summary>
        public string StartCursor { get; set; }

        /// <summary>
        /// The cursor of the last result
        /// </summary>
        public string EndCursor { get; set; }

        /// <summary>
        /// Whether another page can be fetched
        /// </summary>
        public bool HasNextPage { get; set; }

        /// <summary>
        /// Whether another page can be fetched
        /// </summary>
        public bool HasPreviousPage { get; set; }

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
                    HasPreviousPage = HasPreviousPage,
                    StartCursor = StartCursor,
                    EndCursor = EndCursor,
                },
                Edges = Results
                    .Select(x => new Edge<T>
                    {
                        Cursor = x.Cursor,
                        Node = x.Node,
                    })
                    .ToList(),
            };
        }
    }
}