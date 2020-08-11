using System.Linq;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Domain.Models;
using LightOps.Commerce.Proto.Types;
using LightOps.Mapping.Api.Mappers;
using LightOps.Mapping.Api.Services;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Mappers
{
    public class SubNavigationProtoMapper : IMapper<SubNavigationProto, ISubNavigation>
    {
        private readonly IMappingService _mappingService;

        public SubNavigationProtoMapper(IMappingService mappingService)
        {
            _mappingService = mappingService;
        }

        public ISubNavigation Map(SubNavigationProto src)
        {
            return new SubNavigation
            {
                Header = _mappingService.Map<NavigationLinkProto, INavigationLink>(src.Header),
                Links = _mappingService.Map<NavigationLinkProto, INavigationLink>(src.Links).ToList(),
                SubNavigations = _mappingService.Map<SubNavigationProto, ISubNavigation>(src.SubNavigations).ToList(),
            };
        }
    }
}