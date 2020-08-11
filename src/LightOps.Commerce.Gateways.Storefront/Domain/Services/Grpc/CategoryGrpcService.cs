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
    public class CategoryGrpcService : ICategoryService
    {
        private readonly ICategoryEndpointProvider _categoryEndpointProvider;
        private readonly IGrpcCallerService _grpcCallerService;
        private readonly IMappingService _mappingService;

        public CategoryGrpcService(
            ICategoryEndpointProvider categoryEndpointProvider,
            IGrpcCallerService grpcCallerService,
            IMappingService mappingService)
        {
            _categoryEndpointProvider = categoryEndpointProvider;
            _grpcCallerService = grpcCallerService;
            _mappingService = mappingService;
        }

        public async Task<IList<ICategory>> GetByHandleAsync(IList<string> handles)
        {
            return await _grpcCallerService.CallService(_categoryEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new CategoryProtoService.CategoryProtoServiceClient(grpcChannel);
                var request = new GetCategoriesByHandlesProtoRequest();
                request.Handles.AddRange(handles);

                var response = await client.GetCategoriesByHandlesAsync(request);

                return _mappingService
                    .Map<CategoryProto, ICategory>(response.Categories)
                    .ToList();
            });
        }

        public async Task<IList<ICategory>> GetByIdAsync(IList<string> ids)
        {
            return await _grpcCallerService.CallService(_categoryEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new CategoryProtoService.CategoryProtoServiceClient(grpcChannel);
                var request = new GetCategoriesByIdsProtoRequest();
                request.Ids.AddRange(ids);

                var response = await client.GetCategoriesByIdsAsync(request);

                return _mappingService
                    .Map<CategoryProto, ICategory>(response.Categories)
                    .ToList();
            });
        }

        public async Task<SearchResult<ICategory>> GetBySearchAsync(string searchTerm,
                                                                    string parentId,
                                                                    string pageCursor,
                                                                    int pageSize,
                                                                    CategorySortKey sortKey,
                                                                    bool reverse)
        {
            return await _grpcCallerService.CallService(_categoryEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new CategoryProtoService.CategoryProtoServiceClient(grpcChannel);
                var response = await client.GetCategoriesBySearchAsync(new GetCategoriesBySearchProtoRequest
                {
                    SearchTerm = searchTerm,
                    ParentId = parentId,
                    PageCursor = pageCursor,
                    PageSize = pageSize,
                    SortKey = ConvertSortKey(sortKey),
                    Reverse = reverse,
                });

                return new SearchResult<ICategory>
                {
                    HasNextPage = response.HasNextPage,
                    NextPageCursor = response.NextPageCursor,
                    TotalResults = response.TotalResults,
                    Results = _mappingService.Map<CategoryProto, ICategory>(response.Results).ToList()
                };
            });
        }

        private CategorySortKeyProto ConvertSortKey(CategorySortKey sortKey)
        {
            switch (sortKey)
            {
                case CategorySortKey.Default:
                    return CategorySortKeyProto.Default;
                case CategorySortKey.Title:
                    return CategorySortKeyProto.Title;
                case CategorySortKey.CreatedAt:
                    return CategorySortKeyProto.CreatedAt;
                case CategorySortKey.UpdatedAt:
                    return CategorySortKeyProto.UpdatedAt;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}