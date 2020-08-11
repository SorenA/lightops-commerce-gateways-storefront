using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Domain.Models;
using LightOps.Commerce.Proto.Types;
using LightOps.Mapping.Api.Mappers;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Mappers
{
    public class NavigationLinkProtoMapper : IMapper<NavigationLinkProto, INavigationLink>
    {
        public INavigationLink Map(NavigationLinkProto source)
        {
            return new NavigationLink
            {
                Title = source.Title,
                Url = source.Url,
                Target = source.Target,
            };
        }
    }
}