using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Domain.Models;
using LightOps.Commerce.Proto.Services.Navigation.V1;
using LightOps.Mapping.Api.Mappers;

// ReSharper disable UseObjectOrCollectionInitializer

namespace LightOps.Commerce.Gateways.Storefront.Domain.Mappers.V1
{
    public class ProtoNavigationLinkMapper : IMapper<ProtoNavigationLink, INavigationLink>
    {
        public INavigationLink Map(ProtoNavigationLink source)
        {
            var dest = new NavigationLink();

            dest.Title = source.Title;
            dest.Url = source.Url;
            dest.Target = source.Target;

            return dest;
        }
    }
}