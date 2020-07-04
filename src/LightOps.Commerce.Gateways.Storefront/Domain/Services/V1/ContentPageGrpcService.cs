using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Net.Client;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Proto.Services.ContentPage.V1;
using LightOps.Mapping.Api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Services.V1
{
    public class ContentPageGrpcService : IContentPageService
    {
        private readonly IContentPageEndpointProvider _contentPageEndpointProvider;
        private readonly IGrpcCallerService _grpcCallerService;

        public ContentPageGrpcService(
            IContentPageEndpointProvider contentPageEndpointProvider,
            IGrpcCallerService grpcCallerService)
        {
            _contentPageEndpointProvider = contentPageEndpointProvider;
            _grpcCallerService = grpcCallerService;
        }

        private ProtoContentPageService.ProtoContentPageServiceClient GetGrpcClient()
        {
            var grpcChannel = GrpcChannel.ForAddress(_contentPageEndpointProvider.GrpcEndpoint);
            return new ProtoContentPageService.ProtoContentPageServiceClient(grpcChannel);
        }

        public async Task<IContentPage> GetByIdAsync(string id)
        {
            return await _grpcCallerService.CallService(_contentPageEndpointProvider.GrpcEndpoint, async (grpcChannel, provider) =>
            {
                var client = new ProtoContentPageService.ProtoContentPageServiceClient(grpcChannel);
                var response = await client.GetContentPageAsync(new ProtoGetContentPageRequest
                {
                    Id = id,
                });

                var mappingService = provider.GetService<IMappingService>();
                return mappingService
                    .Map<ProtoContentPage, IContentPage>(response.ContentPage);
            });
        }

        public async Task<IContentPage> GetByHandleAsync(string handle)
        {
            return await _grpcCallerService.CallService(_contentPageEndpointProvider.GrpcEndpoint, async (grpcChannel, provider) =>
            {
                var client = new ProtoContentPageService.ProtoContentPageServiceClient(grpcChannel);
                var response = await client.GetContentPageAsync(new ProtoGetContentPageRequest
                {
                    Handle = handle,
                });

                var mappingService = provider.GetService<IMappingService>();
                return mappingService
                    .Map<ProtoContentPage, IContentPage>(response.ContentPage);
            });
        }

        public async Task<IList<IContentPage>> GetByRootAsync()
        {
            return await _grpcCallerService.CallService(_contentPageEndpointProvider.GrpcEndpoint, async (grpcChannel, provider) =>
            {
                var client = new ProtoContentPageService.ProtoContentPageServiceClient(grpcChannel);
                var response = await client.GetContentPagesByRootAsync(new ProtoGetContentPagesByRootRequest());

                var mappingService = provider.GetService<IMappingService>();
                return mappingService
                    .Map<ProtoContentPage, IContentPage>(response.ContentPages)
                    .ToList();
            });
        }

        public async Task<IList<IContentPage>> GetByParentIdAsync(string parentId)
        {
            return await _grpcCallerService.CallService(_contentPageEndpointProvider.GrpcEndpoint, async (grpcChannel, provider) =>
            {
                var client = new ProtoContentPageService.ProtoContentPageServiceClient(grpcChannel);
                var response = await client.GetContentPagesByParentIdAsync(new ProtoGetContentPagesByParentIdRequest
                {
                    ParentId = parentId,
                });

                var mappingService = provider.GetService<IMappingService>();
                return mappingService
                    .Map<ProtoContentPage, IContentPage>(response.ContentPages)
                    .ToList();
            });
        }

        public async Task<IList<IContentPage>> GetBySearchAsync(string searchTerm)
        {
            return await _grpcCallerService.CallService(_contentPageEndpointProvider.GrpcEndpoint, async (grpcChannel, provider) =>
            {
                var client = new ProtoContentPageService.ProtoContentPageServiceClient(grpcChannel);
                var response = await client.GetContentPagesBySearchAsync(new ProtoGetContentPagesBySearchRequest
                {
                    SearchTerm = searchTerm,
                });

                var mappingService = provider.GetService<IMappingService>();
                return mappingService
                    .Map<ProtoContentPage, IContentPage>(response.ContentPages)
                    .ToList();
            });
        }
    }
}