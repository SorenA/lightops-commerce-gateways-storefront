using System.Linq;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Domain.Models;
using LightOps.Commerce.Proto.Types;
using LightOps.Mapping.Api.Mappers;
using LightOps.Mapping.Api.Services;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Mappers
{
    public class NavigationProtoMapper : IMapper<NavigationProto, INavigation>
    {
        private readonly IMappingService _mappingService;

        public NavigationProtoMapper(IMappingService mappingService)
        {
            _mappingService = mappingService;
        }

        public INavigation Map(NavigationProto src)
        {
            return new Navigation
            {
                Id = src.Id,
                ParentId = src.ParentId,
                Handle = src.Handle,
                Type = src.Type,
                CreatedAt = src.CreatedAt.ToDateTime(),
                UpdatedAt = src.UpdatedAt.ToDateTime(),
                Header = _mappingService.Map<NavigationLinkProto, INavigationLink>(src.Header),
                Links = _mappingService.Map<NavigationLinkProto, INavigationLink>(src.Links).ToList(),
                SubNavigations = _mappingService.Map<SubNavigationProto, ISubNavigation>(src.SubNavigations).ToList(),
            };
        }
    }
}