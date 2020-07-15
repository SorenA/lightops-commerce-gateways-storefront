using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types;
using LightOps.Mapping.Api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Queries
{
    public class StorefrontGraphQuery : ObjectGraphType
    {
        private readonly IContentPageEndpointProvider _contentPageEndpointProvider;
        private readonly INavigationEndpointProvider _navigationEndpointProvider;
        private readonly ICategoryEndpointProvider _categoryEndpointProvider;
        private readonly IProductEndpointProvider _productEndpointProvider;
        private readonly IContentPageService _contentPageService;
        private readonly INavigationService _navigationService;
        private readonly ICategoryService _categoryService;
        private readonly IProductService _productService;

        public StorefrontGraphQuery(
            IContentPageEndpointProvider contentPageEndpointProvider,
            INavigationEndpointProvider navigationEndpointProvider,
            ICategoryEndpointProvider categoryEndpointProvider,
            IProductEndpointProvider productEndpointProvider,
            IContentPageService contentPageService,
            INavigationService navigationService,
            ICategoryService categoryService,
            IProductService productService)
        {
            _contentPageEndpointProvider = contentPageEndpointProvider;
            _navigationEndpointProvider = navigationEndpointProvider;
            _categoryEndpointProvider = categoryEndpointProvider;
            _productEndpointProvider = productEndpointProvider;
            _contentPageService = contentPageService;
            _navigationService = navigationService;
            _categoryService = categoryService;
            _productService = productService;

            Name = "Query";

            AddContentPageFields();
            AddNavigationFields();
            AddCategoryFields();
            AddProductFields();
        }

        #region Content Pages
        private void AddContentPageFields()
        {
            Field<ContentPageGraphType>("ContentPage",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "id" },
                    new QueryArgument<StringGraphType> { Name = "handle" }
                ),
                resolve: ResolveContentPage);

            Field<ListGraphType<ContentPageGraphType>>("ContentPages",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "parentId" },
                    new QueryArgument<StringGraphType> { Name = "searchTerm" }
                ),
                resolve: ResolveContentPages);
        }

        private Task<IContentPage> ResolveContentPage(IResolveFieldContext<object> context)
        {
            if (!_contentPageEndpointProvider.IsEnabled)
            {
                throw new ExecutionError("Content pages not supported.");
            }

            if (context.HasArgument("id"))
            {
                return _contentPageService.GetByIdAsync(context.GetArgument<string>("id"));
            }

            if (context.HasArgument("handle"))
            {
                return _contentPageService.GetByHandleAsync(context.GetArgument<string>("handle"));
            }

            return null;
        }

        private Task<IList<IContentPage>> ResolveContentPages(IResolveFieldContext<object> context)
        {
            if (!_contentPageEndpointProvider.IsEnabled)
            {
                throw new ExecutionError("Content pages not supported.");
            }

            if (context.HasArgument("parentId"))
            {
                return _contentPageService.GetByParentIdAsync(context.GetArgument<string>("parentId"));
            }

            if (context.HasArgument("searchTerm"))
            {
                return _contentPageService.GetBySearchAsync(context.GetArgument<string>("searchTerm"));
            }

            // Fallback to root
            return _contentPageService.GetByRootAsync();
        }
        #endregion Content Pages

        #region Navigations
        private void AddNavigationFields()
        {
            Field<NavigationGraphType>("Navigation",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "id" },
                    new QueryArgument<StringGraphType> { Name = "handle" }
                ),
                resolve: ResolveNavigation);

            Field<ListGraphType<NavigationGraphType>>("Navigations",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "parentId" }
                ),
                resolve: ResolveNavigations);
        }

        private Task<INavigation> ResolveNavigation(IResolveFieldContext<object> context)
        {
            if (!_navigationEndpointProvider.IsEnabled)
            {
                throw new ExecutionError("Navigations not supported.");
            }
            
            if (context.HasArgument("id"))
            {
                return _navigationService.GetByIdAsync(context.GetArgument<string>("id"));
            }

            if (context.HasArgument("handle"))
            {
                return _navigationService.GetByHandleAsync(context.GetArgument<string>("handle"));
            }

            return null;
        }

        private Task<IList<INavigation>> ResolveNavigations(IResolveFieldContext<object> context)
        {
            if (!_navigationEndpointProvider.IsEnabled)
            {
                throw new ExecutionError("Navigations not supported.");
            }

            if (context.HasArgument("parentId"))
            {
                return _navigationService.GetByParentIdAsync(context.GetArgument<string>("parentId"));
            }

            // Fallback to root
            return _navigationService.GetByRootAsync();
        }
        #endregion Navigations

        #region Categories
        private void AddCategoryFields()
        {
            Field<CategoryGraphType>("Category",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "id" },
                    new QueryArgument<StringGraphType> { Name = "handle" }
                ),
                resolve: ResolveCategory);

            Field<ListGraphType<CategoryGraphType>>("Categories",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "parentId" },
                    new QueryArgument<StringGraphType> { Name = "searchTerm" }
                ),
                resolve: ResolveCategories);
        }

        private Task<ICategory> ResolveCategory(IResolveFieldContext<object> context)
        {
            if (!_categoryEndpointProvider.IsEnabled)
            {
                throw new ExecutionError("Categories not supported.");
            }

            if (context.HasArgument("id"))
            {
                return _categoryService.GetByIdAsync(context.GetArgument<string>("id"));
            }

            if (context.HasArgument("handle"))
            {
                return _categoryService.GetByHandleAsync(context.GetArgument<string>("handle"));
            }

            return null;
        }

        private Task<IList<ICategory>> ResolveCategories(IResolveFieldContext<object> context)
        {
            if (!_categoryEndpointProvider.IsEnabled)
            {
                throw new ExecutionError("Categories not supported.");
            }

            if (context.HasArgument("parentId"))
            {
                return _categoryService.GetByParentIdAsync(context.GetArgument<string>("parentId"));
            }

            if (context.HasArgument("searchTerm"))
            {
                return _categoryService.GetBySearchAsync(context.GetArgument<string>("searchTerm"));
            }

            // Fallback to root
            return _categoryService.GetByRootAsync();
        }
        #endregion Categories

        #region Products
        private void AddProductFields()
        {
            Field<ProductGraphType>("Product",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "id" },
                    new QueryArgument<StringGraphType> { Name = "handle" }
                ),
                resolve: ResolveProduct);

            Field<ListGraphType<ProductGraphType>>("Products",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "categoryId" },
                    new QueryArgument<StringGraphType> { Name = "searchTerm" }
                ),
                resolve: ResolveProducts);
        }

        private Task<IProduct> ResolveProduct(IResolveFieldContext<object> context)
        {
            if (!_productEndpointProvider.IsEnabled)
            {
                throw new ExecutionError("Products not supported.");
            }

            if (context.HasArgument("id"))
            {
                return _productService.GetByIdAsync(context.GetArgument<string>("id"));
            }

            if (context.HasArgument("handle"))
            {
                return _productService.GetByHandleAsync(context.GetArgument<string>("handle"));
            }

            return null;
        }

        private Task<IList<IProduct>> ResolveProducts(IResolveFieldContext<object> context)
        {
            if (!_productEndpointProvider.IsEnabled)
            {
                throw new ExecutionError("Products not supported.");
            }

            if (context.HasArgument("categoryId"))
            {
                return _productService.GetByCategoryIdAsync(context.GetArgument<string>("categoryId"));
            }

            if (context.HasArgument("searchTerm"))
            {
                return _productService.GetBySearchAsync(context.GetArgument<string>("searchTerm"));
            }

            return null;
        }
        #endregion Products
    }
}
