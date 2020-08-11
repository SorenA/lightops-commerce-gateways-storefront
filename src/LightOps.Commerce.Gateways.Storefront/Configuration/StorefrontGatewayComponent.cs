using System.Collections.Generic;
using System.Linq;
using GraphQL.DataLoader;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Enum;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Queries;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Schemas;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types;
using LightOps.Commerce.Gateways.Storefront.Domain.Mappers;
using LightOps.Commerce.Gateways.Storefront.Domain.Providers;
using LightOps.Commerce.Gateways.Storefront.Domain.Services;
using LightOps.Commerce.Gateways.Storefront.Domain.Services.Grpc;
using LightOps.Commerce.Proto.Types;
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

            ContentPageLookupService,
            NavigationLookupService,
            MetaFieldLookupService,
            CategoryLookupService,
            ProductLookupService,
        }

        private readonly Dictionary<Services, ServiceRegistration> _services = new Dictionary<Services, ServiceRegistration>
        {
            [Services.GrpcCallerService] = ServiceRegistration.Transient<IGrpcCallerService, GrpcCallerService>(),

            [Services.ContentPageService] = ServiceRegistration.Transient<IContentPageService, ContentPageGrpcService>(),
            [Services.NavigationService] = ServiceRegistration.Transient<INavigationService, NavigationGrpcService>(),
            [Services.MetaFieldService] = ServiceRegistration.Transient<IMetaFieldService, MetaFieldGrpcService>(),
            [Services.CategoryService] = ServiceRegistration.Transient<ICategoryService, CategoryGrpcService>(),
            [Services.ProductService] = ServiceRegistration.Transient<IProductService, ProductGrpcService>(),

            [Services.ContentPageLookupService] = ServiceRegistration.Transient<IContentPageLookupService, ContentPageLookupService>(),
            [Services.NavigationLookupService] = ServiceRegistration.Transient<INavigationLookupService, NavigationLookupService>(),
            [Services.MetaFieldLookupService] = ServiceRegistration.Transient<IMetaFieldLookupService, MetaFieldLookupService>(),
            [Services.CategoryLookupService] = ServiceRegistration.Transient<ICategoryLookupService, CategoryLookupService>(),
            [Services.ProductLookupService] = ServiceRegistration.Transient<IProductLookupService, ProductLookupService>(),
        };
        #endregion Services

        #region Mappers
        internal enum Mappers
        {
            ImageProtoMapper,
            MoneyProtoMapper,

            ContentPageProtoMapper,

            NavigationProtoMapper,
            SubNavigationProtoMapper,
            NavigationLinkProtoMapper,

            MetaFieldProtoMapper,

            CategoryProtoMapper,

            ProductProtoMapper,
            ProductVariantProtoMapper,
        }

        private readonly Dictionary<Mappers, ServiceRegistration> _mappers = new Dictionary<Mappers, ServiceRegistration>
        {
            [Mappers.ImageProtoMapper] = ServiceRegistration.Transient<IMapper<ImageProto, IImage>, ImageProtoMapper>(),
            [Mappers.MoneyProtoMapper] = ServiceRegistration.Transient<IMapper<MoneyProto, Money>, MoneyProtoMapper>(),

            [Mappers.ContentPageProtoMapper] = ServiceRegistration.Transient<IMapper<ContentPageProto, IContentPage>, ContentPageProtoMapper>(),

            [Mappers.NavigationProtoMapper] = ServiceRegistration.Transient<IMapper<NavigationProto, INavigation>, NavigationProtoMapper>(),
            [Mappers.SubNavigationProtoMapper] = ServiceRegistration.Transient<IMapper<SubNavigationProto, ISubNavigation>, SubNavigationProtoMapper>(),
            [Mappers.NavigationLinkProtoMapper] = ServiceRegistration.Transient<IMapper<NavigationLinkProto, INavigationLink>, NavigationLinkProtoMapper>(),

            [Mappers.MetaFieldProtoMapper] = ServiceRegistration.Transient<IMapper<MetaFieldProto, IMetaField>, MetaFieldProtoMapper>(),

            [Mappers.CategoryProtoMapper] = ServiceRegistration.Transient<IMapper<CategoryProto, ICategory>, CategoryProtoMapper>(),

            [Mappers.ProductProtoMapper] = ServiceRegistration.Transient<IMapper<ProductProto, IProduct>, ProductProtoMapper>(),
            [Mappers.ProductVariantProtoMapper] = ServiceRegistration.Transient<IMapper<ProductVariantProto, IProductVariant>, ProductVariantProtoMapper>(),
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

            // Enums
            ContentPageSortKeyGraphType,
            CategorySortKeyGraphType,
            ProductSortKeyGraphType,

            // Types
            ImageGraphType,
            MoneyGraphType,

            ContentPageGraphType,

            NavigationGraphType,
            SubNavigationGraphType,
            NavigationLinkGraphType,

            MetaFieldGraphType,

            CategoryGraphType,

            ProductGraphType,
            ProductVariantGraphType,
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

            // Enums
            [Graph.ContentPageSortKeyGraphType] = ServiceRegistration.Singleton<ContentPageSortKeyGraphType, ContentPageSortKeyGraphType>(),
            [Graph.CategorySortKeyGraphType] = ServiceRegistration.Singleton<CategorySortKeyGraphType, CategorySortKeyGraphType>(),
            [Graph.ProductSortKeyGraphType] = ServiceRegistration.Singleton<ProductSortKeyGraphType, ProductSortKeyGraphType>(),

            // Types
            [Graph.ImageGraphType] = ServiceRegistration.Singleton<ImageGraphType, ImageGraphType>(),
            [Graph.MoneyGraphType] = ServiceRegistration.Singleton<MoneyGraphType, MoneyGraphType>(),

            [Graph.ContentPageGraphType] = ServiceRegistration.Singleton<ContentPageGraphType, ContentPageGraphType>(),

            [Graph.NavigationGraphType] = ServiceRegistration.Singleton<NavigationGraphType, NavigationGraphType>(),
            [Graph.SubNavigationGraphType] = ServiceRegistration.Singleton<SubNavigationGraphType, SubNavigationGraphType>(),
            [Graph.NavigationLinkGraphType] = ServiceRegistration.Singleton<NavigationLinkGraphType, NavigationLinkGraphType>(),

            [Graph.MetaFieldGraphType] = ServiceRegistration.Singleton<MetaFieldGraphType, MetaFieldGraphType>(),

            [Graph.CategoryGraphType] = ServiceRegistration.Singleton<CategoryGraphType, CategoryGraphType>(),

            [Graph.ProductGraphType] = ServiceRegistration.Singleton<ProductGraphType, ProductGraphType>(),
            [Graph.ProductVariantGraphType] = ServiceRegistration.Singleton<ProductVariantGraphType, ProductVariantGraphType>(),
        };
        #endregion GraphQL
    }
}