using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Enums;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Enum;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Queries
{
    public sealed class StorefrontGraphQuery : ObjectGraphType
    {
        public StorefrontGraphQuery(
            IDataLoaderContextAccessor dataLoaderContextAccessor,
            IContentPageEndpointProvider contentPageEndpointProvider,
            INavigationEndpointProvider navigationEndpointProvider,
            ICategoryEndpointProvider categoryEndpointProvider,
            IProductEndpointProvider productEndpointProvider,
            IContentPageService contentPageService,
            IContentPageLookupService contentPageLookupService,
            INavigationLookupService navigationLookupService,
            ICategoryService categoryService,
            ICategoryLookupService categoryLookupService,
            IProductService productService,
            IProductLookupService productLookupService)
        {
            Name = "Query";

            #region Content Pages

            Field<ContentPageGraphType, IContentPage>()
                .Name("ContentPage")
                .Argument<StringGraphType>("id", "Id of the content page")
                .Argument<StringGraphType>("handle", "Handle of the content page")
                .ResolveAsync(async ctx =>
                {
                    if (!contentPageEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Content pages not supported.");
                    }

                    if (ctx.HasArgument("id"))
                    {
                        var loader = dataLoaderContextAccessor.Context
                            .GetOrAddBatchLoader<string, IContentPage>("ContentPage.LookupByIdAsync", contentPageLookupService.LookupByIdAsync);
                        return await loader.LoadAsync(ctx.GetArgument<string>("id"));
                    }

                    if (ctx.HasArgument("handle"))
                    {
                        var loader = dataLoaderContextAccessor.Context
                            .GetOrAddBatchLoader<string, IContentPage>("ContentPage.LookupByHandleAsync", contentPageLookupService.LookupByHandleAsync);
                        return await loader.LoadAsync(ctx.GetArgument<string>("handle"));
                    }

                    return null;
                });

            Connection<ContentPageGraphType>()
                .Name("ContentPages")
                .Unidirectional()
                .Argument<StringGraphType>("query", "The search query to filter results by")
                .Argument<ContentPageSortKeyGraphType>("sortKey", "The key to sort the underlying list by")
                .Argument<StringGraphType>("reverse", "Reverse the order of the underlying list")
                .ResolveAsync(async ctx =>
                {
                    if (!contentPageEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Content pages not supported.");
                    }

                    var result = await contentPageService.GetBySearchAsync(
                        ctx.GetArgument<string>("query"),
                        null,
                        ctx.GetArgument<string>("after"),
                        ctx.GetArgument<int>("first", 24),
                        ctx.GetArgument<ContentPageSortKey>("sortKey"),
                        ctx.GetArgument<bool>("reverse"));

                    return result.ToGraphConnection();
                });

            #endregion Content Pages

            #region Navigations

            Field<NavigationGraphType, INavigation>()
                .Name("Navigation")
                .Argument<StringGraphType>("id", "Id of the navigation")
                .Argument<StringGraphType>("handle", "Handle of the navigation")
                .ResolveAsync(async ctx =>
                {
                    if (!navigationEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Navigations not supported.");
                    }

                    if (ctx.HasArgument("id"))
                    {
                        var loader = dataLoaderContextAccessor.Context
                            .GetOrAddBatchLoader<string, INavigation>("Navigation.LookupByIdAsync", navigationLookupService.LookupByIdAsync);
                        return await loader.LoadAsync(ctx.GetArgument<string>("id"));
                    }

                    if (ctx.HasArgument("handle"))
                    {
                        var loader = dataLoaderContextAccessor.Context
                            .GetOrAddBatchLoader<string, INavigation>("Navigation.LookupByHandleAsync", navigationLookupService.LookupByHandleAsync);
                        return await loader.LoadAsync(ctx.GetArgument<string>("handle"));
                    }

                    return null;
                });

            #endregion Navigations

            #region Categories

            Field<CategoryGraphType, ICategory>()
                .Name("Category")
                .Argument<StringGraphType>("id", "Id of the category")
                .Argument<StringGraphType>("handle", "Handle of the category")
                .ResolveAsync(async ctx =>
                {
                    if (!categoryEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Categories not supported.");
                    }

                    if (ctx.HasArgument("id"))
                    {
                        var loader = dataLoaderContextAccessor.Context
                            .GetOrAddBatchLoader<string, ICategory>("Category.LookupByIdAsync", categoryLookupService.LookupByIdAsync);
                        return await loader.LoadAsync(ctx.GetArgument<string>("id"));
                    }

                    if (ctx.HasArgument("handle"))
                    {
                        var loader = dataLoaderContextAccessor.Context
                            .GetOrAddBatchLoader<string, ICategory>("Category.LookupByHandleAsync", categoryLookupService.LookupByHandleAsync);
                        return await loader.LoadAsync(ctx.GetArgument<string>("handle"));
                    }

                    return null;
                });

            Connection<CategoryGraphType>()
                .Name("Categories")
                .Unidirectional()
                .Argument<StringGraphType>("query", "The search query to filter results by")
                .Argument<CategorySortKeyGraphType>("sortKey", "The key to sort the underlying list by")
                .Argument<StringGraphType>("reverse", "Reverse the order of the underlying list")
                .ResolveAsync(async ctx =>
                {
                    if (!categoryEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Categories not supported.");
                    }

                    var result = await categoryService.GetBySearchAsync(
                        ctx.GetArgument<string>("query"),
                        null,
                        ctx.GetArgument<string>("after"),
                        ctx.GetArgument<int>("first", 24),
                        ctx.GetArgument<CategorySortKey>("sortKey"),
                        ctx.GetArgument<bool>("reverse"));

                    return result.ToGraphConnection();
                });

            #endregion Categories

            #region Products

            Field<ProductGraphType, IProduct>()
                .Name("Product")
                .Argument<StringGraphType>("id", "Id of the category")
                .Argument<StringGraphType>("handle", "Handle of the category")
                .ResolveAsync(async ctx =>
                {
                    if (!productEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Products not supported.");
                    }

                    if (ctx.HasArgument("id"))
                    {
                        var loader = dataLoaderContextAccessor.Context
                            .GetOrAddBatchLoader<string, IProduct>("Product.LookupByIdAsync", productLookupService.LookupByIdAsync);
                        return await loader.LoadAsync(ctx.GetArgument<string>("id"));
                    }

                    if (ctx.HasArgument("handle"))
                    {
                        var loader = dataLoaderContextAccessor.Context
                            .GetOrAddBatchLoader<string, IProduct>("Product.LookupByHandleAsync", productLookupService.LookupByHandleAsync);
                        return await loader.LoadAsync(ctx.GetArgument<string>("handle"));
                    }

                    return null;
                });

            Connection<ProductGraphType>()
                .Name("Products")
                .Unidirectional()
                .Argument<StringGraphType>("query", "The search query to filter results by")
                .Argument<ProductSortKeyGraphType>("sortKey", "The key to sort the underlying list by")
                .Argument<StringGraphType>("reverse", "Reverse the order of the underlying list")
                .ResolveAsync(async ctx =>
                {
                    if (!productEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Products not supported.");
                    }

                    var result = await productService.GetBySearchAsync(
                        ctx.GetArgument<string>("query"),
                        null,
                        ctx.GetArgument<string>("after"),
                        ctx.GetArgument<int>("first", 24),
                        ctx.GetArgument<ProductSortKey>("sortKey"),
                        ctx.GetArgument<bool>("reverse"));

                    return result.ToGraphConnection();
                });

            #endregion Products
        }
    }
}
