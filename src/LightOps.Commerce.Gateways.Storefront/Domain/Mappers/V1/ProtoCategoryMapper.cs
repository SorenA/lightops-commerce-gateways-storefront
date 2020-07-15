using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Domain.Models;
using LightOps.Commerce.Proto.Services.Category.V1;
using LightOps.Mapping.Api.Mappers;

// ReSharper disable UseObjectOrCollectionInitializer

namespace LightOps.Commerce.Gateways.Storefront.Domain.Mappers.V1
{
    public class ProtoCategoryMapper : IMapper<ProtoCategory, ICategory>
    {
        public ICategory Map(ProtoCategory source)
        {
            var dest = new Category();

            dest.Id = source.Id;
            dest.Handle = source.Handle;
            dest.Url = source.Url;

            dest.ParentId = source.ParentCategoryId;

            dest.Title = source.Title;
            dest.Description = source.Description;

            dest.SeoTitle = source.SeoTitle;
            dest.SeoDescription = source.SeoDescription;

            dest.PrimaryImage = source.PrimaryImage;

            return dest;
        }
    }
}