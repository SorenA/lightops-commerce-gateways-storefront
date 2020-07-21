using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Proto.Services.Product.V1;
using LightOps.Mapping.Api.Services;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Services.V1
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

        public async Task<IProduct> GetByIdAsync(string id)
        {
            return await _grpcCallerService.CallService(_productEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoProductService.ProtoProductServiceClient(grpcChannel);
                var response = await client.GetProductAsync(new ProtoGetProductRequest
                {
                    Id = id,
                });

                return _mappingService
                    .Map<ProtoProduct, IProduct>(response.Product);
            });
        }

        public async Task<IList<IProduct>> GetByIdAsync(IList<string> ids)
        {
            return await _grpcCallerService.CallService(_productEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoProductService.ProtoProductServiceClient(grpcChannel);
                var request = new GetProductsByIdsRequest();
                request.Ids.AddRange(ids);

                var response = await client.GetProductsByIdsAsync(request);

                return _mappingService
                    .Map<ProtoProduct, IProduct>(response.Products)
                    .ToList();
            });
        }

        public async Task<IProduct> GetByHandleAsync(string handle)
        {
            return await _grpcCallerService.CallService(_productEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoProductService.ProtoProductServiceClient(grpcChannel);
                var response = await client.GetProductAsync(new ProtoGetProductRequest
                {
                    Handle = handle,
                });

                return _mappingService
                    .Map<ProtoProduct, IProduct>(response.Product);
            });
        }

        public async Task<IList<IProduct>> GetByHandleAsync(IList<string> handles)
        {
            return await _grpcCallerService.CallService(_productEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoProductService.ProtoProductServiceClient(grpcChannel);
                var request = new GetProductsByHandlesRequest();
                request.Handles.AddRange(handles);

                var response = await client.GetProductsByHandlesAsync(request);

                return _mappingService
                    .Map<ProtoProduct, IProduct>(response.Products)
                    .ToList();
            });
        }

        public async Task<IList<IProduct>> GetByCategoryIdAsync(string categoryId)
        {
            return await _grpcCallerService.CallService(_productEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoProductService.ProtoProductServiceClient(grpcChannel);
                var response = await client.GetProductsByCategoryIdAsync(new ProtoGetProductsByCategoryIdRequest
                {
                    CategoryId = categoryId,
                });

                return _mappingService
                    .Map<ProtoProduct, IProduct>(response.Products)
                    .ToList();
            });
        }

        public async Task<IDictionary<string, IList<IProduct>>> GetByCategoryIdAsync(IList<string> categoryIds)
        {
            return await _grpcCallerService.CallService(_productEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoProductService.ProtoProductServiceClient(grpcChannel);
                var request = new ProtoGetProductsByCategoryIdsRequest();
                request.CategoryIds.AddRange(categoryIds);

                var response = await client.GetProductsByCategoryIdsAsync(request);

                var dictionary = new Dictionary<string, IList<IProduct>>();
                foreach (var categoryId in response.Products.Keys)
                {
                    var products = _mappingService
                        .Map<ProtoProduct, IProduct>(response.Products[categoryId].Products)
                        .ToList(); 

                    dictionary.Add(categoryId, products);
                }

                return dictionary;
            });
        }

        public async Task<IList<IProduct>> GetBySearchAsync(string searchTerm)
        {
            return await _grpcCallerService.CallService(_productEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoProductService.ProtoProductServiceClient(grpcChannel);
                var response = await client.GetProductsBySearchAsync(new ProtoGetProductsBySearchRequest
                {
                    SearchTerm = searchTerm,
                });

                return _mappingService
                    .Map<ProtoProduct, IProduct>(response.Products)
                    .ToList();
            });
        }
    }
}