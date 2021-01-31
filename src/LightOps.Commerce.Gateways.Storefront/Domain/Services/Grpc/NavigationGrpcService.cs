using System.Collections.Generic;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Proto.Services.Navigation;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Services.Grpc
{
    public class NavigationGrpcService : INavigationService
    {
        private readonly INavigationServiceProvider _navigationServiceProvider;
        private readonly IGrpcCallerService _grpcCallerService;

        public NavigationGrpcService(
            INavigationServiceProvider navigationServiceProvider,
            IGrpcCallerService grpcCallerService)
        {
            _navigationServiceProvider = navigationServiceProvider;
            _grpcCallerService = grpcCallerService;
        }

        public async Task<IList<Navigation>> GetByHandleAsync(IList<string> handles)
        {
            return await _grpcCallerService.CallService(_navigationServiceProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new NavigationService.NavigationServiceClient(grpcChannel);
                var response = await client.GetByHandlesAsync(new GetByHandlesRequest
                {
                    Handles = {handles}
                });

                return response.Navigations;
            });
        }

        public async Task<IList<Navigation>> GetByIdAsync(IList<string> ids)
        {
            return await _grpcCallerService.CallService(_navigationServiceProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new NavigationService.NavigationServiceClient(grpcChannel);
                var response = await client.GetByIdsAsync(new GetByIdsRequest
                {
                    Ids = {ids}
                });

                return response.Navigations;
            });
        }
    }
}