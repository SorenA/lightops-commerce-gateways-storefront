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
    public class MetaFieldGrpcService : IMetaFieldService
    {
        private readonly IMetaFieldEndpointProvider _metaFieldEndpointProvider;
        private readonly IGrpcCallerService _grpcCallerService;
        private readonly IMappingService _mappingService;

        public MetaFieldGrpcService(
            IMetaFieldEndpointProvider metaFieldEndpointProvider,
            IGrpcCallerService grpcCallerService,
            IMappingService mappingService)
        {
            _metaFieldEndpointProvider = metaFieldEndpointProvider;
            _grpcCallerService = grpcCallerService;
            _mappingService = mappingService;
        }
        
        public async Task<IList<IMetaField>> GetByIdAsync(IList<string> ids)
        {
            return await _grpcCallerService.CallService(_metaFieldEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new MetaFieldProtoService.MetaFieldProtoServiceClient(grpcChannel);
                var request = new GetMetaFieldsByIdsProtoRequest();
                request.Ids.AddRange(ids);

                var response = await client.GetMetaFieldsByIdsAsync(request);

                return _mappingService
                    .Map<MetaFieldProto, IMetaField>(response.MetaFields)
                    .ToList();
            });
        }

        public async Task<IDictionary<string, IList<IMetaField>>> GetByParentIdsAsync(IList<string> parentIds)
        {
            return await _grpcCallerService.CallService(_metaFieldEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new MetaFieldProtoService.MetaFieldProtoServiceClient(grpcChannel);
                var request = new GetMetaFieldsByParentIdsProtoRequest();
                request.ParentIds.AddRange(parentIds);

                var response = await client.GetMetaFieldsByParentIdsAsync(request);

                var map = new Dictionary<string, IList<IMetaField>>();

                // Map meta-field map, grouped by parent id
                foreach (var metaFieldMapEntry in response.MetaFields)
                {
                    var metaFields = _mappingService
                        .Map<MetaFieldProto, IMetaField>(metaFieldMapEntry.Value.MetaFields)
                        .ToList();

                    map.Add(metaFieldMapEntry.Key, metaFields);
                }

                return map;
            });
        }

        public async Task<IList<IMetaField>> GetBySearchAsync(string parentId, string @namespace, string name)
        {
            return await _grpcCallerService.CallService(_metaFieldEndpointProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new MetaFieldProtoService.MetaFieldProtoServiceClient(grpcChannel);
                var response = await client.GetMetaFieldsBySearchAsync(new GetMetaFieldsBySearchProtoRequest
                {
                    ParentId = parentId,
                    Namespace = @namespace,
                    Name = name,
                });
                
                return _mappingService
                    .Map<MetaFieldProto, IMetaField>(response.Results)
                    .ToList();
            });
        }
    }
}