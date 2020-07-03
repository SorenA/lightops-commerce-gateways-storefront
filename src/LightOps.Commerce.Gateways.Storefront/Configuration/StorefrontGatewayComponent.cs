using System.Collections.Generic;
using System.Linq;
using LightOps.DependencyInjection.Api.Configuration;
using LightOps.DependencyInjection.Domain.Configuration;

namespace LightOps.Commerce.Gateways.Storefront.Configuration
{
    public class StorefrontGatewayComponent : IDependencyInjectionComponent, IStorefrontGatewayComponent
    {
        public string Name => "lightops.commerce.gateways.storefront";

        public IReadOnlyList<ServiceRegistration> GetServiceRegistrations()
        {
            return new List<ServiceRegistration>()
                .ToList();
        }
    }
}