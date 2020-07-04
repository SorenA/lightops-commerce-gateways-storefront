using System.Collections.Generic;
using System.Linq;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Domain.Mappers.V1;
using LightOps.DependencyInjection.Api.Configuration;
using LightOps.DependencyInjection.Domain.Configuration;
using LightOps.Mapping.Api.Mappers;

namespace LightOps.Commerce.Gateways.Storefront.Configuration
{
    public class StorefrontGatewayComponent : IDependencyInjectionComponent, IStorefrontGatewayComponent
    {
        public string Name => "lightops.commerce.gateways.storefront";

        public IReadOnlyList<ServiceRegistration> GetServiceRegistrations()
        {
            return new List<ServiceRegistration>()
                .Union(_mappers.Values)
                .ToList();
        }

        #region Mappers
        internal enum Mappers
        {
            ProtoContentPageMapperV1,
        }

        private readonly Dictionary<Mappers, ServiceRegistration> _mappers = new Dictionary<Mappers, ServiceRegistration>
        {
            [Mappers.ProtoContentPageMapperV1] = ServiceRegistration
                .Scoped<IMapper<Proto.Services.ContentPage.V1.ProtoContentPage, IContentPage>, ProtoContentPageMapper>(),
        };
        #endregion Mappers
    }
}