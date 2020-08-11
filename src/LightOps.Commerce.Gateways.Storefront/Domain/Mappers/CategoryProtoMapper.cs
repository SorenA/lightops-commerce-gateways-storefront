using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Domain.Models;
using LightOps.Commerce.Proto.Types;
using LightOps.Mapping.Api.Mappers;
using LightOps.Mapping.Api.Services;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Mappers
{
    public class CategoryProtoMapper : IMapper<CategoryProto, ICategory>
    {
        private readonly IMappingService _mappingService;

        public CategoryProtoMapper(IMappingService mappingService)
        {
            _mappingService = mappingService;
        }

        public ICategory Map(CategoryProto src)
        {
            return new Category
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
                PrimaryImage = _mappingService.Map<ImageProto, IImage>(src.PrimaryImage),
            };
        }
    }
}