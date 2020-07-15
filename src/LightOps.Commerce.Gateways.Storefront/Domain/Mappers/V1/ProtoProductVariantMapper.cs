using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Domain.Models;
using LightOps.Commerce.Proto.Services.Product.V1;
using LightOps.Mapping.Api.Mappers;
using LightOps.Mapping.Api.Services;
using NodaMoney;

// ReSharper disable UseObjectOrCollectionInitializer

namespace LightOps.Commerce.Gateways.Storefront.Domain.Mappers.V1
{
    public class ProtoProductVariantMapper : IMapper<ProtoProductVariant, IProductVariant>
    {
        private readonly IMappingService _mappingService;

        public ProtoProductVariantMapper(IMappingService mappingService)
        {
            _mappingService = mappingService;
        }

        public IProductVariant Map(ProtoProductVariant source)
        {
            var dest = new ProductVariant();

            dest.Id = source.Id;

            dest.ProductId = source.ProductId;

            dest.Title = source.Title;
            dest.Sku = source.Sku;

            dest.Price = _mappingService
                .Map<ProtoMoney, Money>(source.Price);

            return dest;
        }
    }
}