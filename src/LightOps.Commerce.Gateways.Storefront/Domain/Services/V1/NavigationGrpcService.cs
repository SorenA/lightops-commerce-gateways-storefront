using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Proto.Services.Navigation.V1;
using LightOps.Mapping.Api.Services;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Services.V1
{
    public class NavigationGrpcService : INavigationService
    {
        private readonly INavigationEndpointProvider _navigationEndpointProvider;
        private readonly IGrpcCallerService _grpcCallerService;
        private readonly IMappingService _mappingService;

        public NavigationGrpcService(
            INavigationEndpointProvider navigationEndpointProvider,
            IGrpcCallerService grpcCallerService,
            IMappingService mappingService)
        {
            _navigationEndpointProvider = navigationEndpointProvider;
            _grpcCallerService = grpcCallerService;
            _mappingService = mappingService;
        }
        
        public async Task<INavigation> GetByIdAsync(string id)
        {
            return await _grpcCallerService.CallService(_navigationEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoNavigationService.ProtoNavigationServiceClient(grpcChannel);
                var response = await client.GetNavigationAsync(new ProtoGetNavigationRequest
                {
                    Id = id,
                });

                return _mappingService
                    .Map<ProtoNavigation, INavigation>(response.Navigation);
            });
        }

        public async Task<IList<INavigation>> GetByIdAsync(IList<string> ids)
        {
            return await _grpcCallerService.CallService(_navigationEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoNavigationService.ProtoNavigationServiceClient(grpcChannel);
                var request = new GetNavigationsByIdsRequest();
                request.Ids.AddRange(ids);

                var response = await client.GetNavigationsByIdsAsync(request);

                return _mappingService
                    .Map<ProtoNavigation, INavigation>(response.Navigations)
                    .ToList();
            });
        }

        public async Task<INavigation> GetByHandleAsync(string handle)
        {
            return await _grpcCallerService.CallService(_navigationEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoNavigationService.ProtoNavigationServiceClient(grpcChannel);
                var response = await client.GetNavigationAsync(new ProtoGetNavigationRequest
                {
                    Handle = handle,
                });

                return _mappingService
                    .Map<ProtoNavigation, INavigation>(response.Navigation);
            });
        }

        public async Task<IList<INavigation>> GetByHandleAsync(IList<string> handles)
        {
            return await _grpcCallerService.CallService(_navigationEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoNavigationService.ProtoNavigationServiceClient(grpcChannel);
                var request = new GetNavigationsByHandlesRequest();
                request.Handles.AddRange(handles);

                var response = await client.GetNavigationsByHandlesAsync(request);

                return _mappingService
                    .Map<ProtoNavigation, INavigation>(response.Navigations)
                    .ToList();
            });
        }

        public async Task<IList<INavigation>> GetByParentIdAsync(string parentId)
        {
            return await _grpcCallerService.CallService(_navigationEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoNavigationService.ProtoNavigationServiceClient(grpcChannel);
                var response = await client.GetNavigationsByParentIdAsync(new ProtoGetNavigationsByParentIdRequest
                {
                    ParentId = parentId,
                });

                return _mappingService
                    .Map<ProtoNavigation, INavigation>(response.Navigations)
                    .ToList();
            });
        }

        public async Task<IList<INavigation>> GetByParentIdAsync(IList<string> parentIds)
        {
            return await _grpcCallerService.CallService(_navigationEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoNavigationService.ProtoNavigationServiceClient(grpcChannel);
                var request = new ProtoGetNavigationsByParentIdsRequest();
                request.ParentIds.AddRange(parentIds);

                var response = await client.GetNavigationsByParentIdsAsync(request);

                return _mappingService
                    .Map<ProtoNavigation, INavigation>(response.Navigations)
                    .ToList();
            });
        }

        public async Task<IList<INavigation>> GetByRootAsync()
        {
            return await _grpcCallerService.CallService(_navigationEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new ProtoNavigationService.ProtoNavigationServiceClient(grpcChannel);
                var response = await client.GetNavigationsByRootAsync(new ProtoGetNavigationsByRootRequest());

                return _mappingService
                    .Map<ProtoNavigation, INavigation>(response.Navigations)
                    .ToList();
            });
        }
    }
}