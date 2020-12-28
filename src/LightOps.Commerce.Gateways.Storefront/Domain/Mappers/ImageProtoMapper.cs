using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Domain.Models;
using LightOps.Commerce.Proto.Types;
using LightOps.Mapping.Api.Mappers;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Mappers
{
    public class ImageProtoMapper : IMapper<ImageProto, IImage>
    {
        public IImage Map(ImageProto src)
        {
            return new Image
            {
                Id = src.Id,
                Url = src.Url,
                AltText = src.AltText,
                FocalCenterTop = src.FocalCenterTop ?? 0.5,
                FocalCenterLeft = src.FocalCenterLeft ?? 0.5,
            };
        }
    }
}