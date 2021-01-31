using System;
using GraphQL.Server;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Contexts;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Schemas;
using LightOps.DependencyInjection.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace LightOps.Commerce.Gateways.Storefront.Configuration
{
    public static class DependencyInjectionRootComponentExtensions
    {
        public static IDependencyInjectionRootComponent AddStorefrontGateway(this IDependencyInjectionRootComponent rootComponent,
                                                                             IServiceCollection serviceCollection,
                                                                             Action<IStorefrontGatewayComponent> componentConfig = null)
        {
            var component = new StorefrontGatewayComponent();

            // Invoke config delegate
            componentConfig?.Invoke(component);

            // Attach to root component
            rootComponent.AttachComponent(component);

            // Add and configure GraphQL
            serviceCollection.AddGraphQL((options, provider) =>
                {
                    var logger = provider.GetRequiredService<ILogger<StorefrontGraphSchema>>();
                    options.UnhandledExceptionDelegate = ctx =>
                        logger.LogError("{Error} occured", ctx.Exception, ctx.OriginalException.Message);

                    // Pass call to configurator for custom configuration, if configured
                    component.ConfigureGraphQLDelegate?.Invoke(options, provider);
                })
                .AddDataLoader()
                .AddUserContextBuilder<StorefrontGraphUserContextBuilder>()
                .AddSystemTextJson(deserializerSettings => { }, serializerSettings => { });

            return rootComponent;
        }
    }
}
