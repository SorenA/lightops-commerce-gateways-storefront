using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Proto.Services.MetaField;
using LightOps.Commerce.Proto.Types;
using LightOps.Mapping.Api.Services;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Services.Grpc
{
    public class MetaFieldGrpcService : IMetaFieldService
    {
        private readonly IMetaFieldServiceProvider _metaFieldServiceProvider;
        private readonly IGrpcCallerService _grpcCallerService;
        private readonly IMappingService _mappingService;

        public MetaFieldGrpcService(
            IMetaFieldServiceProvider metaFieldServiceProvider,
            IGrpcCallerService grpcCallerService,
            IMappingService mappingService)
        {
            _metaFieldServiceProvider = metaFieldServiceProvider;
            _grpcCallerService = grpcCallerService;
            _mappingService = mappingService;
        }
        
        public async Task<IList<MetaField>> GetByIdAsync(IList<string> ids)
        {
            return await _grpcCallerService.CallService(_metaFieldServiceProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new MetaFieldService.MetaFieldServiceClient(grpcChannel);
                var response = await client.GetByIdsAsync(new GetByIdsRequest
                {
                    Ids = {ids}
                });

                // Filter out non-public meta-fields
                return response.MetaFields
                    .Where(x => x.IsPublic)
                    .ToList();
            });
        }

        public async Task<IDictionary<string, IList<MetaField>>> GetByParentIdsAsync(IList<string> parentIds)
        {
            return await _grpcCallerService.CallService(_metaFieldServiceProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new MetaFieldService.MetaFieldServiceClient(grpcChannel);
                var response = await client.GetByParentIdsAsync(new GetByParentIdsRequest
                {
                    ParentIds = {parentIds}
                });

                return response.MetaFields
                    .ToDictionary(
                        k => k.Key,
                        // Filter out non-public meta-fields
                        v => (IList<MetaField>) v.Value.MetaFields
                            .Where(x => x.IsPublic)
                            .ToList());
            });
        }

        public async Task<IList<MetaField>> GetBySearchAsync(string parentId, string @namespace, string name)
        {
            return await _grpcCallerService.CallService(_metaFieldServiceProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new MetaFieldService.MetaFieldServiceClient(grpcChannel);
                var response = await client.GetBySearchAsync(new GetBySearchRequest
                {
                    ParentId = parentId,
                    Namespace = @namespace,
                    Name = name,
                });

                // Filter out non-public meta-fields
                return response.Results
                    .Where(x => x.IsPublic)
                    .ToList();
            });
        }
    }
}