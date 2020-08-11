using System.Linq;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Domain.Models;
using LightOps.Commerce.Proto.Types;
using LightOps.Mapping.Api.Mappers;
using LightOps.Mapping.Api.Services;
using NodaMoney;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Mappers
{
    public class ProductVariantProtoMapper : IMapper<ProductVariantProto, IProductVariant>
    {
        private readonly IMappingService _mappingService;

        public ProductVariantProtoMapper(IMappingService mappingService)
        {
            _mappingService = mappingService;
        }

        public IProductVariant Map(ProductVariantProto src)
        {
            return new ProductVariant
            {
                Id = src.Id,
                ProductId = src.ProductId,
                Title = src.Title,
                Sku = src.Sku,
                UnitPrice = _mappingService.Map<MoneyProto, Money>(src.UnitPrice),
                Images = _mappingService.Map<ImageProto, IImage>(src.Images).ToList(),
            };
        }
    }
}