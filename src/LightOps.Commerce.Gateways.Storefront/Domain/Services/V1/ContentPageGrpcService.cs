using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Proto.Services.ContentPage.V1;
using LightOps.Mapping.Api.Services;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Services.V1
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

        public async Task<IContentPage> GetByIdAsync(string id)
        {
            return await _grpcCallerService.CallService(_contentPageEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoContentPageService.ProtoContentPageServiceClient(grpcChannel);
                var response = await client.GetContentPageAsync(new ProtoGetContentPageRequest
                {
                    Id = id,
                });

                return _mappingService
                    .Map<ProtoContentPage, IContentPage>(response.ContentPage);
            });
        }

        public async Task<IContentPage> GetByHandleAsync(string handle)
        {
            return await _grpcCallerService.CallService(_contentPageEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoContentPageService.ProtoContentPageServiceClient(grpcChannel);
                var response = await client.GetContentPageAsync(new ProtoGetContentPageRequest
                {
                    Handle = handle,
                });

                return _mappingService
                    .Map<ProtoContentPage, IContentPage>(response.ContentPage);
            });
        }

        public async Task<IList<IContentPage>> GetByIdAsync(IList<string> ids)
        {
            return await _grpcCallerService.CallService(_contentPageEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoContentPageService.ProtoContentPageServiceClient(grpcChannel);
                var request = new GetContentPagesByIdRequest();
                request.Ids.AddRange(ids);

                var response = await client.GetContentPagesByIdAsync(request);

                return _mappingService
                    .Map<ProtoContentPage, IContentPage>(response.ContentPages)
                    .ToList();
            });
        }

        public async Task<IList<IContentPage>> GetByHandleAsync(IList<string> handles)
        {
            return await _grpcCallerService.CallService(_contentPageEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoContentPageService.ProtoContentPageServiceClient(grpcChannel);
                var request = new GetContentPagesByHandleRequest();
                request.Handles.AddRange(handles);

                var response = await client.GetContentPagesByHandleAsync(request);

                return _mappingService
                    .Map<ProtoContentPage, IContentPage>(response.ContentPages)
                    .ToList();
            });
        }

        public async Task<IList<IContentPage>> GetByRootAsync()
        {
            return await _grpcCallerService.CallService(_contentPageEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoContentPageService.ProtoContentPageServiceClient(grpcChannel);
                var response = await client.GetContentPagesByRootAsync(new ProtoGetContentPagesByRootRequest());

                return _mappingService
                    .Map<ProtoContentPage, IContentPage>(response.ContentPages)
                    .ToList();
            });
        }

        public async Task<IList<IContentPage>> GetByParentIdAsync(string parentId)
        {
            return await _grpcCallerService.CallService(_contentPageEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoContentPageService.ProtoContentPageServiceClient(grpcChannel);
                var response = await client.GetContentPagesByParentIdAsync(new ProtoGetContentPagesByParentIdRequest
                {
                    ParentId = parentId,
                });

                return _mappingService
                    .Map<ProtoContentPage, IContentPage>(response.ContentPages)
                    .ToList();
            });
        }

        public async Task<IList<IContentPage>> GetBySearchAsync(string searchTerm)
        {
            return await _grpcCallerService.CallService(_contentPageEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoContentPageService.ProtoContentPageServiceClient(grpcChannel);
                var response = await client.GetContentPagesBySearchAsync(new ProtoGetContentPagesBySearchRequest
                {
                    SearchTerm = searchTerm,
                });

                return _mappingService
                    .Map<ProtoContentPage, IContentPage>(response.ContentPages)
                    .ToList();
            });
        }

        public async Task<ILookup<string, IContentPage>> LookupByIdAsync(IEnumerable<string> ids)
        {
            var result = await GetByIdAsync(ids.ToList());
            return result.ToLookup(x => x.Id);
        }

        public async Task<ILookup<string, IContentPage>> LookupByHandleAsync(IEnumerable<string> handles)
        {
            var result = await GetByIdAsync(handles.ToList());
            return result.ToLookup(x => x.Id);
        }
    }
}