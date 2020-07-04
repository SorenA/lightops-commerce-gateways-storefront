using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Domain.Models;
using LightOps.Commerce.Proto.Services.MetaField.V1;
using LightOps.Mapping.Api.Mappers;

// ReSharper disable UseObjectOrCollectionInitializer

namespace LightOps.Commerce.Gateways.Storefront.Domain.Mappers.V1
{
    public class ProtoMetaFieldMapper : IMapper<ProtoMetaField, IMetaField>
    {
        public IMetaField Map(ProtoMetaField source)
        {
            var dest = new MetaField();

            dest.Name= source.Name;
            dest.Type = source.Type;
            dest.Value = source.Value;

            return dest;
        }
    }
}
