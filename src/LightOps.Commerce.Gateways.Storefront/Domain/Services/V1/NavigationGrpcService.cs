﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Proto.Services.Navigation.V1;
using LightOps.Mapping.Api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Services.V1
{
    public class NavigationGrpcService : INavigationService
    {
        private readonly INavigationEndpointProvider _navigationEndpointProvider;
        private readonly IGrpcCallerService _grpcCallerService;

        public NavigationGrpcService(
            INavigationEndpointProvider navigationEndpointProvider,
            IGrpcCallerService grpcCallerService)
        {
            _navigationEndpointProvider = navigationEndpointProvider;
            _grpcCallerService = grpcCallerService;
        }
        
        public async Task<INavigation> GetByIdAsync(string id)
        {
            return await _grpcCallerService.CallService(_navigationEndpointProvider.GrpcEndpoint, async (grpcChannel, provider) =>
            {
                var client = new ProtoNavigationService.ProtoNavigationServiceClient(grpcChannel);
                var response = await client.GetNavigationAsync(new ProtoGetNavigationRequest
                {
                    Id = id,
                });

                var mappingService = provider.GetService<IMappingService>();
                return mappingService
                    .Map<ProtoNavigation, INavigation>(response.Navigation);
            });
        }

        public async Task<INavigation> GetByHandleAsync(string handle)
        {
            return await _grpcCallerService.CallService(_navigationEndpointProvider.GrpcEndpoint, async (grpcChannel, provider) =>
            {
                var client = new ProtoNavigationService.ProtoNavigationServiceClient(grpcChannel);
                var response = await client.GetNavigationAsync(new ProtoGetNavigationRequest
                {
                    Handle = handle,
                });

                var mappingService = provider.GetService<IMappingService>();
                return mappingService
                    .Map<ProtoNavigation, INavigation>(response.Navigation);
            });
        }

        public async Task<IList<INavigation>> GetByRootAsync()
        {
            return await _grpcCallerService.CallService(_navigationEndpointProvider.GrpcEndpoint, async (grpcChannel, provider) =>
            {
                var client = new ProtoNavigationService.ProtoNavigationServiceClient(grpcChannel);
                var response = await client.GetNavigationsByRootAsync(new ProtoGetNavigationsByRootRequest());

                var mappingService = provider.GetService<IMappingService>();
                return mappingService
                    .Map<ProtoNavigation, INavigation>(response.Navigations)
                    .ToList();
            });
        }

        public async Task<IList<INavigation>> GetByParentIdAsync(string parentId)
        {
            return await _grpcCallerService.CallService(_navigationEndpointProvider.GrpcEndpoint, async (grpcChannel, provider) =>
            {
                var client = new ProtoNavigationService.ProtoNavigationServiceClient(grpcChannel);
                var response = await client.GetNavigationsByParentIdAsync(new ProtoGetNavigationsByParentIdRequest
                {
                    ParentId = parentId,
                });

                var mappingService = provider.GetService<IMappingService>();
                return mappingService
                    .Map<ProtoNavigation, INavigation>(response.Navigations)
                    .ToList();
            });
        }
    }
}