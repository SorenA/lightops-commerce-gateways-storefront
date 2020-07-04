using System.Collections.Generic;
using System.Linq;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Queries;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Schemas;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types;
using LightOps.Commerce.Gateways.Storefront.Domain.Mappers.V1;
using LightOps.Commerce.Gateways.Storefront.Domain.Providers;
using LightOps.Commerce.Gateways.Storefront.Domain.Services;
using LightOps.Commerce.Gateways.Storefront.Domain.Services.V1;
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
                .Union(_services.Values)
                .Union(_mappers.Values)
                .Union(_providers.Values)
                .Union(_graph.Values)
                .ToList();
        }

        #region Services
        internal enum Services
        {
            GrpcCallerService,
            ContentPageService,
            NavigationService,
        }

        private readonly Dictionary<Services, ServiceRegistration> _services = new Dictionary<Services, ServiceRegistration>
        {
            [Services.GrpcCallerService] = ServiceRegistration.Scoped<IGrpcCallerService, GrpcCallerService>(),
            [Services.ContentPageService] = ServiceRegistration.Scoped<IContentPageService, ContentPageGrpcService>(),
            [Services.NavigationService] = ServiceRegistration.Scoped<INavigationService, NavigationGrpcService>(),
        };
        #endregion Services

        #region Mappers
        internal enum Mappers
        {
            ProtoContentPageMapperV1,
            ProtoNavigationMapperV1,
            ProtoNavigationLinkMapperV1,
        }

        private readonly Dictionary<Mappers, ServiceRegistration> _mappers = new Dictionary<Mappers, ServiceRegistration>
        {
            [Mappers.ProtoContentPageMapperV1] = ServiceRegistration
                .Scoped<IMapper<Proto.Services.ContentPage.V1.ProtoContentPage, IContentPage>, ProtoContentPageMapper>(),
            [Mappers.ProtoNavigationMapperV1] = ServiceRegistration
                .Scoped<IMapper<Proto.Services.Navigation.V1.ProtoNavigation, INavigation>, ProtoNavigationMapper>(),
            [Mappers.ProtoNavigationLinkMapperV1] = ServiceRegistration
                .Scoped<IMapper<Proto.Services.Navigation.V1.ProtoNavigationLink, INavigationLink>, ProtoNavigationLinkMapper>(),
        };
        #endregion Mappers

        #region Providers
        internal enum Providers
        {
            ContentPageEndpointProvider,
            NavigationEndpointProvider,
        }

        private readonly Dictionary<Providers, ServiceRegistration> _providers = new Dictionary<Providers, ServiceRegistration>()
        {
            [Providers.ContentPageEndpointProvider] = ServiceRegistration.Singleton<IContentPageEndpointProvider>(new ContentPageEndpointProvider()),
            [Providers.NavigationEndpointProvider] = ServiceRegistration.Singleton<INavigationEndpointProvider>(new NavigationEndpointProvider()),
        };

        public IStorefrontGatewayComponent UseContentPages(string grpcEndpoint)
        {
            _providers[Providers.ContentPageEndpointProvider].ImplementationInstance = new ContentPageEndpointProvider
            {
                IsEnabled = true,
                GrpcEndpoint = grpcEndpoint,
            };
            return this;
        }

        public IStorefrontGatewayComponent UseNavigations(string grpcEndpoint)
        {
            _providers[Providers.NavigationEndpointProvider].ImplementationInstance = new NavigationEndpointProvider
            {
                IsEnabled = true,
                GrpcEndpoint = grpcEndpoint,
            };
            return this;
        }
        #endregion Providers

        #region GraphQL
        internal enum Graph
        {
            // Schemas
            Schema,
            // Queries
            StorefrontGraphQuery,
            // Types
            ContentPageGraphType,
            NavigationGraphType,
            NavigationLinkGraphType,
        }

        private readonly Dictionary<Graph, ServiceRegistration> _graph = new Dictionary<Graph, ServiceRegistration>
        {
            // Schemas
            [Graph.Schema] = ServiceRegistration.Singleton<ISchema, StorefrontGraphSchema>(),
            // Queries
            [Graph.StorefrontGraphQuery] = ServiceRegistration.Singleton<StorefrontGraphQuery, StorefrontGraphQuery>(),
            // Types
            [Graph.ContentPageGraphType] = ServiceRegistration.Singleton<ContentPageGraphType, ContentPageGraphType>(),
            [Graph.NavigationGraphType] = ServiceRegistration.Singleton<NavigationGraphType, NavigationGraphType>(),
            [Graph.NavigationLinkGraphType] = ServiceRegistration.Singleton<NavigationLinkGraphType, NavigationLinkGraphType>(),
        };
        #endregion GraphQL
    }
}