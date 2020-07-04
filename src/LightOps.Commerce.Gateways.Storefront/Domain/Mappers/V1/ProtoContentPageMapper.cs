// ReSharper disable UseObjectOrCollectionInitializer

using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Domain.Models;
using LightOps.Commerce.Proto.Services.ContentPage.V1;
using LightOps.Mapping.Api.Mappers;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Mappers.V1
{
    public class ProtoContentPageMapper : IMapper<ProtoContentPage, IContentPage>
    {
        public IContentPage Map(ProtoContentPage source)
        {
            var dest = new ContentPage();

            dest.Id = source.Id;
            dest.Handle = source.Handle;
            dest.Url = source.Url;

            dest.ParentId = source.ParentContentPageId;

            dest.Title = source.Title;
            dest.Type = source.Type;
            dest.Description = source.Description;

            dest.SeoTitle = source.SeoTitle;
            dest.SeoDescription = source.SeoDescription;

            dest.PrimaryImage = source.PrimaryImage;

            return dest;
        }
    }
}