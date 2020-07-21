using System.Collections.Generic;
using System.Linq;
using GraphQL.DataLoader;
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
using NodaMoney;

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
            MetaFieldService,
            CategoryService,
            ProductService,

            NavigationLookupService,
        }

        private readonly Dictionary<Services, ServiceRegistration> _services = new Dictionary<Services, ServiceRegistration>
        {
            [Services.GrpcCallerService] = ServiceRegistration.Transient<IGrpcCallerService, GrpcCallerService>(),

            [Services.ContentPageService] = ServiceRegistration.Transient<IContentPageService, ContentPageGrpcService>(),
            [Services.NavigationService] = ServiceRegistration.Transient<INavigationService, NavigationGrpcService>(),
            [Services.MetaFieldService] = ServiceRegistration.Transient<IMetaFieldService, MetaFieldGrpcService>(),
            [Services.CategoryService] = ServiceRegistration.Transient<ICategoryService, CategoryGrpcService>(),
            [Services.ProductService] = ServiceRegistration.Transient<IProductService, ProductGrpcService>(),

            [Services.NavigationLookupService] = ServiceRegistration.Transient<INavigationLookupService, NavigationLookupService>(),
        };
        #endregion Services

        #region Mappers
        internal enum Mappers
        {
            ProtoContentPageMapperV1,
            ProtoNavigationMapperV1,
            ProtoNavigationLinkMapperV1,
            ProtoMetaFieldMapperV1,
            ProtoCategoryMapperV1,
            ProtoMoneyMapperV1,
            ProtoProductMapperV1,
            ProtoProductVariantMapperV1,
        }

        private readonly Dictionary<Mappers, ServiceRegistration> _mappers = new Dictionary<Mappers, ServiceRegistration>
        {
            [Mappers.ProtoContentPageMapperV1] = ServiceRegistration
                .Transient<IMapper<Proto.Services.ContentPage.V1.ProtoContentPage, IContentPage>, ProtoContentPageMapper>(),
            [Mappers.ProtoNavigationMapperV1] = ServiceRegistration
                .Transient<IMapper<Proto.Services.Navigation.V1.ProtoNavigation, INavigation>, ProtoNavigationMapper>(),
            [Mappers.ProtoNavigationLinkMapperV1] = ServiceRegistration
                .Transient<IMapper<Proto.Services.Navigation.V1.ProtoNavigationLink, INavigationLink>, ProtoNavigationLinkMapper>(),
            [Mappers.ProtoMetaFieldMapperV1] = ServiceRegistration
                .Transient<IMapper<Proto.Services.MetaField.V1.ProtoMetaField, IMetaField>, ProtoMetaFieldMapper>(),
            [Mappers.ProtoCategoryMapperV1] = ServiceRegistration
                .Transient<IMapper<Proto.Services.Category.V1.ProtoCategory, ICategory>, ProtoCategoryMapper>(),
            [Mappers.ProtoMoneyMapperV1] = ServiceRegistration
                .Transient<IMapper<Proto.Services.Product.V1.ProtoMoney, Money>, ProtoMoneyMapper>(),
            [Mappers.ProtoProductMapperV1] = ServiceRegistration
                .Transient<IMapper<Proto.Services.Product.V1.ProtoProduct, IProduct>, ProtoProductMapper>(),
            [Mappers.ProtoProductVariantMapperV1] = ServiceRegistration
                .Transient<IMapper<Proto.Services.Product.V1.ProtoProductVariant, IProductVariant>, ProtoProductVariantMapper>(),
        };
        #endregion Mappers

        #region Providers
        internal enum Providers
        {
            ContentPageEndpointProvider,
            NavigationEndpointProvider,
            MetaFieldEndpointProvider,
            CategoryEndpointProvider,
            ProductEndpointProvider,
        }

        private readonly Dictionary<Providers, ServiceRegistration> _providers = new Dictionary<Providers, ServiceRegistration>()
        {
            [Providers.ContentPageEndpointProvider] = ServiceRegistration.Singleton<IContentPageEndpointProvider>(new ContentPageEndpointProvider()),
            [Providers.NavigationEndpointProvider] = ServiceRegistration.Singleton<INavigationEndpointProvider>(new NavigationEndpointProvider()),
            [Providers.MetaFieldEndpointProvider] = ServiceRegistration.Singleton<IMetaFieldEndpointProvider>(new MetaFieldEndpointProvider()),
            [Providers.CategoryEndpointProvider] = ServiceRegistration.Singleton<ICategoryEndpointProvider>(new CategoryEndpointProvider()),
            [Providers.ProductEndpointProvider] = ServiceRegistration.Singleton<IProductEndpointProvider>(new ProductEndpointProvider()),
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

        public IStorefrontGatewayComponent UseMetaFields(string grpcEndpoint)
        {
            _providers[Providers.MetaFieldEndpointProvider].ImplementationInstance = new MetaFieldEndpointProvider
            {
                IsEnabled = true,
                GrpcEndpoint = grpcEndpoint,
            };
            return this;
        }

        public IStorefrontGatewayComponent UseCategories(string grpcEndpoint)
        {
            _providers[Providers.CategoryEndpointProvider].ImplementationInstance = new CategoryEndpointProvider
            {
                IsEnabled = true,
                GrpcEndpoint = grpcEndpoint,
            };
            return this;
        }

        public IStorefrontGatewayComponent UseProducts(string grpcEndpoint)
        {
            _providers[Providers.ProductEndpointProvider].ImplementationInstance = new ProductEndpointProvider
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
            // Data loaders
            DataLoaderContextAccessor,
            DataLoaderDocumentListener,
            // Schemas
            Schema,
            // Queries
            StorefrontGraphQuery,
            // Types
            ContentPageGraphType,
            NavigationGraphType,
            NavigationLinkGraphType,
            MetaFieldGraphType,
            CategoryGraphType,
            ProductGraphType,
            ProductVariantGraphType,
            MoneyGraphType,
        }

        private readonly Dictionary<Graph, ServiceRegistration> _graph = new Dictionary<Graph, ServiceRegistration>
        {
            // Data loaders
            [Graph.DataLoaderContextAccessor] = ServiceRegistration.Singleton<IDataLoaderContextAccessor, DataLoaderContextAccessor>(),
            [Graph.DataLoaderDocumentListener] = ServiceRegistration.Singleton<DataLoaderDocumentListener, DataLoaderDocumentListener>(),
            // Schemas
            [Graph.Schema] = ServiceRegistration.Singleton<ISchema, StorefrontGraphSchema>(),
            // Queries
            [Graph.StorefrontGraphQuery] = ServiceRegistration.Singleton<StorefrontGraphQuery, StorefrontGraphQuery>(),
            // Types
            [Graph.ContentPageGraphType] = ServiceRegistration.Singleton<ContentPageGraphType, ContentPageGraphType>(),
            [Graph.NavigationGraphType] = ServiceRegistration.Singleton<NavigationGraphType, NavigationGraphType>(),
            [Graph.NavigationLinkGraphType] = ServiceRegistration.Singleton<NavigationLinkGraphType, NavigationLinkGraphType>(),
            [Graph.MetaFieldGraphType] = ServiceRegistration.Singleton<MetaFieldGraphType, MetaFieldGraphType>(),
            [Graph.CategoryGraphType] = ServiceRegistration.Singleton<CategoryGraphType, CategoryGraphType>(),
            [Graph.ProductGraphType] = ServiceRegistration.Singleton<ProductGraphType, ProductGraphType>(),
            [Graph.ProductVariantGraphType] = ServiceRegistration.Singleton<ProductVariantGraphType, ProductVariantGraphType>(),
            [Graph.MoneyGraphType] = ServiceRegistration.Singleton<MoneyGraphType, MoneyGraphType>(),
        };
        #endregion GraphQL
    }
}