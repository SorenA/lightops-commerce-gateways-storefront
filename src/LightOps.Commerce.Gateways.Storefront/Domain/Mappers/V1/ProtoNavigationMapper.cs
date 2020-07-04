using System.Linq;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Domain.Models;
using LightOps.Commerce.Proto.Services.Navigation.V1;
using LightOps.Mapping.Api.Mappers;
using LightOps.Mapping.Api.Services;

// ReSharper disable UseObjectOrCollectionInitializer

namespace LightOps.Commerce.Gateways.Storefront.Domain.Mappers.V1
{
    public class ProtoNavigationMapper : IMapper<ProtoNavigation, INavigation>
    {
        private readonly IMappingService _mappingService;

        public ProtoNavigationMapper(IMappingService mappingService)
        {
            _mappingService = mappingService;
        }

        public INavigation Map(ProtoNavigation source)
        {
            var dest = new Navigation();

            dest.Id = source.Id;
            dest.Handle = source.Handle;

            dest.ParentNavigationId = source.ParentNavigationId;

            dest.Header = _mappingService
                .Map<ProtoNavigationLink, INavigationLink>(source.Header);

            dest.Links = _mappingService
                .Map<ProtoNavigationLink, INavigationLink>(source.Links)
                .ToList();

            // Can't use IMappingService to resolve self
            dest.SubNavigations = source.SubNavigations
                .Select(Map)
                .ToList();

            return dest;
        }
    }
}