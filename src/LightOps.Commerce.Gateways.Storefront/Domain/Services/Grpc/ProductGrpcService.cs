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
    public class ProductGrpcService : IProductService
    {
        private readonly IProductEndpointProvider _productEndpointProvider;
        private readonly IGrpcCallerService _grpcCallerService;
        private readonly IMappingService _mappingService;

        public ProductGrpcService(
            IProductEndpointProvider productEndpointProvider,
            IGrpcCallerService grpcCallerService,
            IMappingService mappingService)
        {
            _productEndpointProvider = productEndpointProvider;
            _grpcCallerService = grpcCallerService;
            _mappingService = mappingService;
        }

        public async Task<IList<IProduct>> GetByHandleAsync(IList<string> handles)
        {
            return await _grpcCallerService.CallService(_productEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProductProtoService.ProductProtoServiceClient(grpcChannel);
                var request = new GetProductsByHandlesProtoRequest();
                request.Handles.AddRange(handles);

                var response = await client.GetProductsByHandlesAsync(request);

                return _mappingService
                    .Map<ProductProto, IProduct>(response.Products)
                    .ToList();
            });
        }

        public async Task<IList<IProduct>> GetByIdAsync(IList<string> ids)
        {
            return await _grpcCallerService.CallService(_productEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProductProtoService.ProductProtoServiceClient(grpcChannel);
                var request = new GetProductsByIdsProtoRequest();
                request.Ids.AddRange(ids);

                var response = await client.GetProductsByIdsAsync(request);

                return _mappingService
                    .Map<ProductProto, IProduct>(response.Products)
                    .ToList();
            });
        }

        public async Task<SearchResult<IProduct>> GetBySearchAsync(string searchTerm,
                                                                   string categoryId,
                                                                   string pageCursor,
                                                                   int pageSize,
                                                                   ProductSortKey sortKey,
                                                                   bool reverse)
        {
            return await _grpcCallerService.CallService(_productEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProductProtoService.ProductProtoServiceClient(grpcChannel);
                var response = await client.GetProductsBySearchAsync(new GetProductsBySearchProtoRequest()
                {
                    SearchTerm = searchTerm,
                    CategoryId = categoryId,
                    PageCursor = pageCursor,
                    PageSize = pageSize,
                    SortKey = ConvertSortKey(sortKey),
                    Reverse = reverse,
                });

                var results = response
                    .Results
                    .Select(x => new CursorNodeResult<IProduct>
                    {
                        Cursor = x.Cursor,
                        Node = _mappingService.Map<ProductProto, IProduct>(x.Node),
                    })
                    .ToList();

                return new SearchResult<IProduct>
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

        private ProductSortKeyProto ConvertSortKey(ProductSortKey sortKey)
        {
            switch (sortKey)
            {
                case ProductSortKey.Default:
                    return ProductSortKeyProto.Default;
                case ProductSortKey.Title:
                    return ProductSortKeyProto.Title;
                case ProductSortKey.CreatedAt:
                    return ProductSortKeyProto.CreatedAt;
                case ProductSortKey.UpdatedAt:
                    return ProductSortKeyProto.UpdatedAt;
                case ProductSortKey.UnitPrice:
                    return ProductSortKeyProto.UnitPrice;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}