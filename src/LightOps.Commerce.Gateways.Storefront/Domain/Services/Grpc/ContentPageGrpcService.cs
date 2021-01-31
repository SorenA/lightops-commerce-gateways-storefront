using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.QueryResults;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Proto.Services.ContentPage;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Services.Grpc
{
    public class ContentPageGrpcService : IContentPageService
    {
        private readonly IContentPageServiceProvider _contentPageServiceProvider;
        private readonly IGrpcCallerService _grpcCallerService;

        public ContentPageGrpcService(
            IContentPageServiceProvider contentPageServiceProvider,
            IGrpcCallerService grpcCallerService)
        {
            _contentPageServiceProvider = contentPageServiceProvider;
            _grpcCallerService = grpcCallerService;
        }
        
        public async Task<IList<ContentPage>> GetByHandleAsync(IList<string> handles)
        {
            return await _grpcCallerService.CallService(_contentPageServiceProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ContentPageService.ContentPageServiceClient(grpcChannel);
                var response = await client.GetByHandlesAsync(new GetByHandlesRequest
                {
                    Handles = {handles}
                });

                return response.ContentPages;
            });
        }

        public async Task<IList<ContentPage>> GetByIdAsync(IList<string> ids)
        {
            return await _grpcCallerService.CallService(_contentPageServiceProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ContentPageService.ContentPageServiceClient(grpcChannel);
                var response = await client.GetByIdsAsync(new GetByIdsRequest
                {
                    Ids = {ids}
                });

                return response.ContentPages;
            });
        }

        public async Task<SearchResult<ContentPage>> GetBySearchAsync(string searchTerm,
                                                                      string languageCode,
                                                                      string parentId,
                                                                      string pageCursor,
                                                                      int pageSize,
                                                                      ContentPageSortKey sortKey,
                                                                      bool reverse)
        {
            return await _grpcCallerService.CallService(_contentPageServiceProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ContentPageService.ContentPageServiceClient(grpcChannel);
                var response = await client.GetBySearchAsync(new GetBySearchRequest
                {
                    SearchTerm = searchTerm,
                    LanguageCode = languageCode,
                    ParentId = parentId,
                    PageCursor = pageCursor,
                    PageSize = pageSize,
                    SortKey = sortKey,
                    Reverse = reverse,
                });

                var results = response
                    .Results
                    .Select(x => new CursorNodeResult<ContentPage>
                    {
                        Cursor = x.Cursor,
                        Node = x.Node
                    })
                    .ToList();

                return new SearchResult<ContentPage>
                {
                    HasNextPage = response.HasNextPage,
                    HasPreviousPage = response.HasPreviousPage,
                    StartCursor = response.StartCursor,
                    EndCursor = response.EndCursor,
                    TotalResults = response.TotalResults,
                    Results = results,
                };
            });
        }
    }
}