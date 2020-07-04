using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Proto.Services.MetaField.V1;
using LightOps.Mapping.Api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Services.V1
{
    public class MetaFieldGrpcService : IMetaFieldService
    {
        private readonly IMetaFieldEndpointProvider _metaFieldEndpointProvider;
        private readonly IGrpcCallerService _grpcCallerService;

        public MetaFieldGrpcService(
            IMetaFieldEndpointProvider metaFieldEndpointProvider,
            IGrpcCallerService grpcCallerService)
        {
            _metaFieldEndpointProvider = metaFieldEndpointProvider;
            _grpcCallerService = grpcCallerService;
        }
        
        public async Task<IMetaField> GetByParentAsync(string parentEntityType, string parentEntityId, string name)
        {
            return await _grpcCallerService.CallService(_metaFieldEndpointProvider.GrpcEndpoint, async (grpcChannel, provider) =>
            {
                var client = new ProtoMetaFieldService.ProtoMetaFieldServiceClient(grpcChannel);
                var response = await client.GetMetaFieldByParentAsync(new ProtoGetMetaFieldByParentRequest
                {
                    ParentEntityType = parentEntityType,
                    ParentEntityId = parentEntityId,
                    Name = name,
                });

                var mappingService = provider.GetService<IMappingService>();
                return mappingService
                    .Map<ProtoMetaField, IMetaField>(response.MetaField);
            });
        }

        public async Task<IList<IMetaField>> GetByParentAsync(string parentEntityType, string parentEntityId)
        {
            return await _grpcCallerService.CallService(_metaFieldEndpointProvider.GrpcEndpoint, async (grpcChannel, provider) =>
            {
                var client = new ProtoMetaFieldService.ProtoMetaFieldServiceClient(grpcChannel);
                var response = await client.GetMetaFieldsByParentAsync(new ProtoGetMetaFieldsByParentRequest
                {
                    ParentEntityType = parentEntityType,
                    ParentEntityId = parentEntityId,
                });

                var mappingService = provider.GetService<IMappingService>();
                return mappingService
                    .Map<ProtoMetaField, IMetaField>(response.MetaFields)
                    .ToList();
            });
        }
    }
}