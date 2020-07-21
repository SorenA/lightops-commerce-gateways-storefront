using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Proto.Services.Category.V1;
using LightOps.Mapping.Api.Services;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Services.V1
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
        
        public async Task<ICategory> GetByIdAsync(string id)
        {
            return await _grpcCallerService.CallService(_categoryEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoCategoryService.ProtoCategoryServiceClient(grpcChannel);
                var response = await client.GetCategoryAsync(new ProtoGetCategoryRequest
                {
                    Id = id,
                });

                return _mappingService
                    .Map<ProtoCategory, ICategory>(response.Category);
            });
        }

        public async Task<IList<ICategory>> GetByIdAsync(IList<string> ids)
        {
            return await _grpcCallerService.CallService(_categoryEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoCategoryService.ProtoCategoryServiceClient(grpcChannel);
                var request = new GetCategoriesByIdsRequest();
                request.Ids.AddRange(ids);

                var response = await client.GetCategoriesByIdsAsync(request);

                return _mappingService
                    .Map<ProtoCategory, ICategory>(response.Categories)
                    .ToList();
            });
        }

        public async Task<ICategory> GetByHandleAsync(string handle)
        {
            return await _grpcCallerService.CallService(_categoryEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoCategoryService.ProtoCategoryServiceClient(grpcChannel);
                var response = await client.GetCategoryAsync(new ProtoGetCategoryRequest
                {
                    Handle = handle,
                });

                return _mappingService
                    .Map<ProtoCategory, ICategory>(response.Category);
            });
        }

        public async Task<IList<ICategory>> GetByHandleAsync(IList<string> handles)
        {
            return await _grpcCallerService.CallService(_categoryEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoCategoryService.ProtoCategoryServiceClient(grpcChannel);
                var request = new GetCategoriesByHandlesRequest();
                request.Handles.AddRange(handles);

                var response = await client.GetCategoriesByHandlesAsync(request);

                return _mappingService
                    .Map<ProtoCategory, ICategory>(response.Categories)
                    .ToList();
            });
        }

        public async Task<IList<ICategory>> GetByParentIdAsync(string parentId)
        {
            return await _grpcCallerService.CallService(_categoryEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoCategoryService.ProtoCategoryServiceClient(grpcChannel);
                var response = await client.GetCategoriesByParentIdAsync(new ProtoGetCategoriesByParentIdRequest
                {
                    ParentId = parentId,
                });

                return _mappingService
                    .Map<ProtoCategory, ICategory>(response.Categories)
                    .ToList();
            });
        }

        public async Task<IList<ICategory>> GetByParentIdAsync(IList<string> parentIds)
        {
            return await _grpcCallerService.CallService(_categoryEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoCategoryService.ProtoCategoryServiceClient(grpcChannel);
                var request = new ProtoGetCategoriesByParentIdsRequest();
                request.ParentIds.AddRange(parentIds);

                var response = await client.GetCategoriesByParentIdsAsync(request);

                return _mappingService
                    .Map<ProtoCategory, ICategory>(response.Categories)
                    .ToList();
            });
        }

        public async Task<IList<ICategory>> GetByRootAsync()
        {
            return await _grpcCallerService.CallService(_categoryEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoCategoryService.ProtoCategoryServiceClient(grpcChannel);
                var response = await client.GetCategoriesByRootAsync(new ProtoGetCategoriesByRootRequest());

                return _mappingService
                    .Map<ProtoCategory, ICategory>(response.Categories)
                    .ToList();
            });
        }

        public async Task<IList<ICategory>> GetBySearchAsync(string searchTerm)
        {
            return await _grpcCallerService.CallService(_categoryEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoCategoryService.ProtoCategoryServiceClient(grpcChannel);
                var response = await client.GetCategoriesBySearchAsync(new ProtoGetCategoriesBySearchRequest
                {
                    SearchTerm = searchTerm,
                });

                return _mappingService
                    .Map<ProtoCategory, ICategory>(response.Categories)
                    .ToList();
            });
        }
    }
}