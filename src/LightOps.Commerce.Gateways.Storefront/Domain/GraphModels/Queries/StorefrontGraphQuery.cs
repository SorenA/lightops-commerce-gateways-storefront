using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Contexts;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Enum;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Queries
{
    public sealed class StorefrontGraphQuery : ObjectGraphType
    {
        public StorefrontGraphQuery(
            IDataLoaderContextAccessor dataLoaderContextAccessor,
            IContentPageServiceProvider contentPageServiceProvider,
            INavigationServiceProvider navigationServiceProvider,
            ICategoryServiceProvider categoryServiceProvider,
            IProductServiceProvider productServiceProvider,
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

            Field<ContentPageGraphType, ContentPage>()
                .Name("ContentPage")
                .Argument<StringGraphType>("id", "Id of the content page")
                .Argument<StringGraphType>("handle", "Handle of the content page")
                .ResolveAsync(ctx =>
                {
                    if (!contentPageServiceProvider.IsEnabled)
                    {
                        throw new ExecutionError("Content pages not supported.");
                    }

                    if (ctx.HasArgument("id"))
                    {
                        var loader = dataLoaderContextAccessor.Context
                            .GetOrAddBatchLoader<string, ContentPage>("ContentPage.LookupByIdAsync", contentPageLookupService.LookupByIdAsync);
                        return loader.LoadAsync(ctx.GetArgument<string>("id"));
                    }

                    if (ctx.HasArgument("handle"))
                    {
                        var loader = dataLoaderContextAccessor.Context
                            .GetOrAddBatchLoader<string, ContentPage>("ContentPage.LookupByHandleAsync", contentPageLookupService.LookupByHandleAsync);
                        return loader.LoadAsync(ctx.GetArgument<string>("handle"));
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
                    if (!contentPageServiceProvider.IsEnabled)
                    {
                        throw new ExecutionError("Content pages not supported.");
                    }

                    var userContext = (StorefrontGraphUserContext) ctx.UserContext;

                    var result = await contentPageService.GetBySearchAsync(
                        ctx.GetArgument<string>("query"),
                        userContext.LanguageCode,
                        null,
                        ctx.GetArgument<string>("after"),
                        ctx.GetArgument<int>("first", 24),
                        ctx.GetArgument<ContentPageSortKey>("sortKey"),
                        ctx.GetArgument<bool>("reverse"));

                    return result.ToGraphConnection();
                });

            #endregion Content Pages

            #region Navigations

            Field<NavigationGraphType, Navigation>()
                .Name("Navigation")
                .Argument<StringGraphType>("id", "Id of the navigation")
                .Argument<StringGraphType>("handle", "Handle of the navigation")
                .ResolveAsync(ctx =>
                {
                    if (!navigationServiceProvider.IsEnabled)
                    {
                        throw new ExecutionError("Navigations not supported.");
                    }

                    if (ctx.HasArgument("id"))
                    {
                        var loader = dataLoaderContextAccessor.Context
                            .GetOrAddBatchLoader<string, Navigation>("Navigation.LookupByIdAsync", navigationLookupService.LookupByIdAsync);
                        return loader.LoadAsync(ctx.GetArgument<string>("id"));
                    }

                    if (ctx.HasArgument("handle"))
                    {
                        var loader = dataLoaderContextAccessor.Context
                            .GetOrAddBatchLoader<string, Navigation>("Navigation.LookupByHandleAsync", navigationLookupService.LookupByHandleAsync);
                        return loader.LoadAsync(ctx.GetArgument<string>("handle"));
                    }

                    return null;
                });

            #endregion Navigations

            #region Categories

            Field<CategoryGraphType, Category>()
                .Name("Category")
                .Argument<StringGraphType>("id", "Id of the category")
                .Argument<StringGraphType>("handle", "Handle of the category")
                .ResolveAsync(ctx =>
                {
                    if (!categoryServiceProvider.IsEnabled)
                    {
                        throw new ExecutionError("Categories not supported.");
                    }

                    if (ctx.HasArgument("id"))
                    {
                        var loader = dataLoaderContextAccessor.Context
                            .GetOrAddBatchLoader<string, Category>("Category.LookupByIdAsync", categoryLookupService.LookupByIdAsync);
                        return loader.LoadAsync(ctx.GetArgument<string>("id"));
                    }

                    if (ctx.HasArgument("handle"))
                    {
                        var loader = dataLoaderContextAccessor.Context
                            .GetOrAddBatchLoader<string, Category>("Category.LookupByHandleAsync", categoryLookupService.LookupByHandleAsync);
                        return loader.LoadAsync(ctx.GetArgument<string>("handle"));
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
                    if (!categoryServiceProvider.IsEnabled)
                    {
                        throw new ExecutionError("Categories not supported.");
                    }

                    var userContext = (StorefrontGraphUserContext)ctx.UserContext;

                    var result = await categoryService.GetBySearchAsync(
                        ctx.GetArgument<string>("query"),
                        userContext.LanguageCode,
                        null,
                        ctx.GetArgument<string>("after"),
                        ctx.GetArgument<int>("first", 24),
                        ctx.GetArgument<CategorySortKey>("sortKey"),
                        ctx.GetArgument<bool>("reverse"));

                    return result.ToGraphConnection();
                });

            #endregion Categories

            #region Products

            Field<ProductGraphType, Product>()
                .Name("Product")
                .Argument<StringGraphType>("id", "Id of the category")
                .Argument<StringGraphType>("handle", "Handle of the category")
                .ResolveAsync(ctx =>
                {
                    if (!productServiceProvider.IsEnabled)
                    {
                        throw new ExecutionError("Products not supported.");
                    }

                    if (ctx.HasArgument("id"))
                    {
                        var loader = dataLoaderContextAccessor.Context
                            .GetOrAddBatchLoader<string, Product>("Product.LookupByIdAsync", productLookupService.LookupByIdAsync);
                        return loader.LoadAsync(ctx.GetArgument<string>("id"));
                    }

                    if (ctx.HasArgument("handle"))
                    {
                        var loader = dataLoaderContextAccessor.Context
                            .GetOrAddBatchLoader<string, Product>("Product.LookupByHandleAsync", productLookupService.LookupByHandleAsync);
                        return loader.LoadAsync(ctx.GetArgument<string>("handle"));
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
                    if (!productServiceProvider.IsEnabled)
                    {
                        throw new ExecutionError("Products not supported.");
                    }

                    var userContext = (StorefrontGraphUserContext)ctx.UserContext;

                    var result = await productService.GetBySearchAsync(
                        ctx.GetArgument<string>("query"),
                        userContext.LanguageCode,
                        null,
                        ctx.GetArgument<string>("after"),
                        ctx.GetArgument<int>("first", 24),
                        ctx.GetArgument<ProductSortKey>("sortKey"),
                        ctx.GetArgument<bool>("reverse"),
                        userContext.CurrencyCode);

                    return result.ToGraphConnection();
                });

            #endregion Products
        }
    }
}
