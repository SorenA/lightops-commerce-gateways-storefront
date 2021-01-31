using System.Collections.Generic;
using System.Linq;
using GraphQL.DataLoader;
using GraphQL.Server.Transports.AspNetCore;
using GraphQL.Types;
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
            MoneyProtoMapper,
        }

        private readonly Dictionary<Mappers, ServiceRegistration> _mappers = new Dictionary<Mappers, ServiceRegistration>
        {
            [Mappers.MoneyProtoMapper] = ServiceRegistration.Transient<IMapper<Proto.Types.Money, NodaMoney.Money>, MoneyProtoMapper>(),
        };
        #endregion Mappers

        #region Providers
        internal enum Providers
        {
            ImageCdnProvider,

            ContentPageServiceProvider,
            NavigationServiceProvider,
            MetaFieldServiceProvider,
            CategoryServiceProvider,
            ProductServiceProvider,
        }

        private readonly Dictionary<Providers, ServiceRegistration> _providers = new Dictionary<Providers, ServiceRegistration>()
        {
            [Providers.ImageCdnProvider] = ServiceRegistration.Singleton<IImageCdnProvider>(new ImageCdnProvider()),

            [Providers.ContentPageServiceProvider] = ServiceRegistration.Singleton<IContentPageServiceProvider>(new ContentPageServiceProvider()),
            [Providers.NavigationServiceProvider] = ServiceRegistration.Singleton<INavigationServiceProvider>(new NavigationServiceProvider()),
            [Providers.MetaFieldServiceProvider] = ServiceRegistration.Singleton<IMetaFieldServiceProvider>(new MetaFieldServiceProvider()),
            [Providers.CategoryServiceProvider] = ServiceRegistration.Singleton<ICategoryServiceProvider>(new CategoryServiceProvider()),
            [Providers.ProductServiceProvider] = ServiceRegistration.Singleton<IProductServiceProvider>(new ProductServiceProvider()),
        };

        public IStorefrontGatewayComponent UseImageCdn(string cdnHost)
        {
            _providers[Providers.ImageCdnProvider].ImplementationInstance = new ImageCdnProvider
            {
                IsEnabled = true,
                CdnHost = cdnHost,
            };
            return this;
        }

        public IStorefrontGatewayComponent UseContentPages(string grpcEndpoint)
        {
            _providers[Providers.ContentPageServiceProvider].ImplementationInstance = new ContentPageServiceProvider
            {
                IsEnabled = true,
                GrpcEndpoint = grpcEndpoint,
            };
            return this;
        }

        public IStorefrontGatewayComponent UseNavigations(string grpcEndpoint)
        {
            _providers[Providers.NavigationServiceProvider].ImplementationInstance = new NavigationServiceProvider
            {
                IsEnabled = true,
                GrpcEndpoint = grpcEndpoint,
            };
            return this;
        }

        public IStorefrontGatewayComponent UseMetaFields(string grpcEndpoint)
        {
            _providers[Providers.MetaFieldServiceProvider].ImplementationInstance = new MetaFieldServiceProvider
            {
                IsEnabled = true,
                GrpcEndpoint = grpcEndpoint,
            };
            return this;
        }

        public IStorefrontGatewayComponent UseCategories(string grpcEndpoint)
        {
            _providers[Providers.CategoryServiceProvider].ImplementationInstance = new CategoryServiceProvider
            {
                IsEnabled = true,
                GrpcEndpoint = grpcEndpoint,
            };
            return this;
        }

        public IStorefrontGatewayComponent UseProducts(string grpcEndpoint)
        {
            _providers[Providers.ProductServiceProvider].ImplementationInstance = new ProductServiceProvider
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