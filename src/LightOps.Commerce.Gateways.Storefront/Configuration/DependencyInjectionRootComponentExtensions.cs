using System;
using LightOps.DependencyInjection.Configuration;

namespace LightOps.Commerce.Gateways.Storefront.Configuration
{
    public static class DependencyInjectionRootComponentExtensions
    {
        public static IDependencyInjectionRootComponent AddStorefrontGateway(this IDependencyInjectionRootComponent rootComponent, Action<IStorefrontGatewayComponent> componentConfig = null)
        {
            var component = new StorefrontGatewayComponent();

            // Invoke config delegate
            componentConfig?.Invoke(component);

            // Attach to root component
            rootComponent.AttachComponent(component);

            return rootComponent;
        }
    }
}
