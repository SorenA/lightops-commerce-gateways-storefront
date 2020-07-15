using System.Linq;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Domain.Models;
using LightOps.Commerce.Proto.Services.Product.V1;
using LightOps.Mapping.Api.Mappers;
using LightOps.Mapping.Api.Services;

// ReSharper disable UseObjectOrCollectionInitializer

namespace LightOps.Commerce.Gateways.Storefront.Domain.Mappers.V1
{
    public class ProtoProductMapper : IMapper<ProtoProduct, IProduct>
    {
        private readonly IMappingService _mappingService;

        public ProtoProductMapper(IMappingService mappingService)
        {
            _mappingService = mappingService;
        }

        public IProduct Map(ProtoProduct source)
        {
            var dest = new Product();

            dest.Id = source.Id;
            dest.Handle = source.Handle;
            dest.Url = source.Url;

            dest.Title= source.Title;
            dest.Type = source.Type;
            dest.Description = source.Description;

            dest.SeoTitle = source.SeoTitle;
            dest.SeoDescription = source.SeoDescription;

            dest.PrimaryCategoryId = source.PrimaryCategoryId;
            dest.CategoryIds = source.CategoryIds.ToList();

            dest.Variants = _mappingService
                .Map<ProtoProductVariant, IProductVariant>(source.Variants)
                .ToList();

            dest.PrimaryImage = source.PrimaryImage;
            dest.Images = source.Images.ToList();

            return dest;
        }
    }
}