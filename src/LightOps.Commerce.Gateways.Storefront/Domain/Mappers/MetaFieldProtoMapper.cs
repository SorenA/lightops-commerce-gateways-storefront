using Google.Protobuf.WellKnownTypes;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Domain.Models;
using LightOps.Commerce.Proto.Types;
using LightOps.Mapping.Api.Mappers;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Mappers
{
    public class MetaFieldProtoMapper : IMapper<MetaFieldProto, IMetaField>
    {
        public IMetaField Map(MetaFieldProto src)
        {
            return new MetaField
            {
                Id = src.Id,
                ParentId = src.ParentId,
                Namespace = src.Namespace,
                Name = src.Name,
                Type = src.Type,
                Value = src.Value,
                CreatedAt = src.CreatedAt.ToDateTime(),
                UpdatedAt = src.UpdatedAt.ToDateTime(),
            };
        }
    }
}
