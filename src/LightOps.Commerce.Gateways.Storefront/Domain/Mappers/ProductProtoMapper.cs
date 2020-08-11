using System.Linq;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Domain.Models;
using LightOps.Commerce.Proto.Types;
using LightOps.Mapping.Api.Mappers;
using LightOps.Mapping.Api.Services;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Mappers
{
    public class ProductProtoMapper : IMapper<ProductProto, IProduct>
    {
        private readonly IMappingService _mappingService;

        public ProductProtoMapper(IMappingService mappingService)
        {
            _mappingService = mappingService;
        }

        public IProduct Map(ProductProto src)
        {
            return new Product
            {
                Id = src.Id,
                ParentId = src.ParentId,
                Handle = src.Handle,
                Title = src.Title,
                Url = src.Url,
                Type = src.Type,
                Description = src.Description,
                CreatedAt = src.CreatedAt.ToDateTime(),
                UpdatedAt = src.UpdatedAt.ToDateTime(),
                PrimaryCategoryId = src.PrimaryCategoryId,
                CategoryIds = src.CategoryIds.ToList(),
                Variants = _mappingService.Map<ProductVariantProto, IProductVariant>(src.Variants).ToList(),
                Images = _mappingService.Map<ImageProto, IImage>(src.Images).ToList(),
            };
        }
    }
}