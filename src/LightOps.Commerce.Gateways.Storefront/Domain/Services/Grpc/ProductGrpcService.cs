using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.QueryResults;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Proto.Services.Product;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Services.Grpc
{
    public class ProductGrpcService : IProductService
    {
        private readonly IProductServiceProvider _productServiceProvider;
        private readonly IGrpcCallerService _grpcCallerService;

        public ProductGrpcService(
            IProductServiceProvider productServiceProvider,
            IGrpcCallerService grpcCallerService)
        {
            _productServiceProvider = productServiceProvider;
            _grpcCallerService = grpcCallerService;
        }

        public async Task<IList<Product>> GetByHandleAsync(IList<string> handles)
        {
            return await _grpcCallerService.CallService(_productServiceProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProductService.ProductServiceClient(grpcChannel);
                var response = await client.GetByHandlesAsync(new GetByHandlesRequest
                {
                    Handles = {handles}
                });

                return response.Products;
            });
        }

        public async Task<IList<Product>> GetByIdAsync(IList<string> ids)
        {
            return await _grpcCallerService.CallService(_productServiceProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProductService.ProductServiceClient(grpcChannel);
                var response = await client.GetByIdsAsync(new GetByIdsRequest
                {
                    Ids = {ids}
                });

                return response.Products;
            });
        }

        public async Task<SearchResult<Product>> GetBySearchAsync(string searchTerm,
                                                                  string languageCode,
                                                                  string categoryId,
                                                                  string pageCursor,
                                                                  int pageSize,
                                                                  ProductSortKey sortKey,
                                                                  bool reverse,
                                                                  string currencyCode)
        {
            return await _grpcCallerService.CallService(_productServiceProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProductService.ProductServiceClient(grpcChannel);
                var response = await client.GetBySearchAsync(new GetBySearchRequest
                {
                    SearchTerm = searchTerm,
                    LanguageCode = languageCode,
                    CategoryId = categoryId,
                    PageCursor = pageCursor,
                    PageSize = pageSize,
                    SortKey = sortKey,
                    Reverse = reverse,
                    CurrencyCode = currencyCode,
                });

                var results = response
                    .Results
                    .Select(x => new CursorNodeResult<Product>
                    {
                        Cursor = x.Cursor,
                        Node = x.Node,
                    })
                    .ToList();

                return new SearchResult<Product>
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