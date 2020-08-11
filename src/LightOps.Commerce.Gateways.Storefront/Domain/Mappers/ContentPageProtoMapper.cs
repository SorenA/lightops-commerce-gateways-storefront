using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Domain.Models;
using LightOps.Commerce.Proto.Types;
using LightOps.Mapping.Api.Mappers;
using LightOps.Mapping.Api.Services;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Mappers
{
    public class ContentPageProtoMapper : IMapper<ContentPageProto, IContentPage>
    {
        private readonly IMappingService _mappingService;

        public ContentPageProtoMapper(IMappingService mappingService)
        {
            _mappingService = mappingService;
        }

        public IContentPage Map(ContentPageProto src)
        {
            return new ContentPage
            {
                Id = src.Id,
                ParentId = src.ParentId,
                Handle = src.Handle,
                Title = src.Title,
                Url = src.Url,
                Type = src.Type,
                Summary = src.Summary,
                CreatedAt = src.CreatedAt.ToDateTime(),
                UpdatedAt = src.UpdatedAt.ToDateTime(),
                PrimaryImage = _mappingService.Map<ImageProto, IImage>(src.PrimaryImage),
            };
        }
    }
}