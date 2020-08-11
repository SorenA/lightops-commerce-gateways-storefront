using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Proto.Services;
using LightOps.Commerce.Proto.Types;
using LightOps.Mapping.Api.Services;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Services.Grpc
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

        public async Task<IList<INavigation>> GetByHandleAsync(IList<string> handles)
        {
            return await _grpcCallerService.CallService(_navigationEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new NavigationProtoService.NavigationProtoServiceClient(grpcChannel);
                var request = new GetNavigationsByHandlesProtoRequest();
                request.Handles.AddRange(handles);

                var response = await client.GetNavigationsByHandlesAsync(request);

                return _mappingService
                    .Map<NavigationProto, INavigation>(response.Navigations)
                    .ToList();
            });
        }

        public async Task<IList<INavigation>> GetByIdAsync(IList<string> ids)
        {
            return await _grpcCallerService.CallService(_navigationEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new NavigationProtoService.NavigationProtoServiceClient(grpcChannel);
                var request = new GetNavigationsByIdsProtoRequest();
                request.Ids.AddRange(ids);

                var response = await client.GetNavigationsByIdsAsync(request);

                return _mappingService
                    .Map<NavigationProto, INavigation>(response.Navigations)
                    .ToList();
            });
        }
    }
}