using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Enums;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.QueryResults;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Proto.Services;
using LightOps.Commerce.Proto.Types;
using LightOps.Mapping.Api.Services;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Services.Grpc
{
    public class ContentPageGrpcService : IContentPageService
    {
        private readonly IContentPageEndpointProvider _contentPageEndpointProvider;
        private readonly IGrpcCallerService _grpcCallerService;
        private readonly IMappingService _mappingService;

        public ContentPageGrpcService(
            IContentPageEndpointProvider contentPageEndpointProvider,
            IGrpcCallerService grpcCallerService,
            IMappingService mappingService)
        {
            _contentPageEndpointProvider = contentPageEndpointProvider;
            _grpcCallerService = grpcCallerService;
            _mappingService = mappingService;
        }

        public async Task<IList<IContentPage>> GetByHandleAsync(IList<string> handles)
        {
            return await _grpcCallerService.CallService(_contentPageEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ContentPageProtoService.ContentPageProtoServiceClient(grpcChannel);
                var request = new GetContentPagesByHandlesProtoRequest();
                request.Handles.AddRange(handles);

                var response = await client.GetContentPagesByHandlesAsync(request);

                return _mappingService
                    .Map<ContentPageProto, IContentPage>(response.ContentPages)
                    .ToList();
            });
        }

        public async Task<IList<IContentPage>> GetByIdAsync(IList<string> ids)
        {
            return await _grpcCallerService.CallService(_contentPageEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ContentPageProtoService.ContentPageProtoServiceClient(grpcChannel);
                var request = new GetContentPagesByIdsProtoRequest();
                request.Ids.AddRange(ids);

                var response = await client.GetContentPagesByIdsAsync(request);

                return _mappingService
                    .Map<ContentPageProto, IContentPage>(response.ContentPages)
                    .ToList();
            });
        }

        public async Task<SearchResult<IContentPage>> GetBySearchAsync(string searchTerm,
                                                                string parentId,
                                                                string pageCursor,
                                                                int pageSize,
                                                                ContentPageSortKey sortKey,
                                                                bool reverse)
        {
            return await _grpcCallerService.CallService(_contentPageEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ContentPageProtoService.ContentPageProtoServiceClient(grpcChannel);
                var response = await client.GetContentPagesBySearchAsync(new GetContentPagesBySearchProtoRequest
                {
                    SearchTerm = searchTerm,
                    ParentId = parentId,
                    PageCursor = pageCursor,
                    PageSize = pageSize,
                    SortKey = ConvertSortKey(sortKey),
                    Reverse = reverse,
                });

                var results = response
                    .Results
                    .Select(x => new CursorNodeResult<IContentPage>
                    {
                        Cursor = x.Cursor,
                        Node = _mappingService.Map<ContentPageProto, IContentPage>(x.Node),
                    })
                    .ToList();

                return new SearchResult<IContentPage>
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

        private ContentPageSortKeyProto ConvertSortKey(ContentPageSortKey sortKey)
        {
            switch (sortKey)
            {
                case ContentPageSortKey.Default:
                    return ContentPageSortKeyProto.Default;
                case ContentPageSortKey.Title:
                    return ContentPageSortKeyProto.Title;
                case ContentPageSortKey.CreatedAt:
                    return ContentPageSortKeyProto.CreatedAt;
                case ContentPageSortKey.UpdatedAt:
                    return ContentPageSortKeyProto.UpdatedAt;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}