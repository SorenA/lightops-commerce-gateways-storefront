using System.Collections.Generic;
using System.Linq;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class CategoryGraphType : ObjectGraphType<ICategory>
    {
        public CategoryGraphType(
            IDataLoaderContextAccessor dataLoaderContextAccessor,
            IMetaFieldEndpointProvider metaFieldEndpointProvider,
            IMetaFieldService metaFieldService,
            IProductEndpointProvider productEndpointProvider,
            IProductService productService,
            ICategoryService categoryService
            )
        {
            Name = "Category";

            Field(m => m.Id);
            Field(m => m.Handle);
            Field(m => m.Url);

            Field(m => m.ParentId, true);

            Field(m => m.Title);
            Field(m => m.Type);
            Field(m => m.Description);

            Field(m => m.SeoTitle);
            Field(m => m.SeoDescription);

            Field(m => m.PrimaryImage);

            // Meta-fields
            Field<MetaFieldGraphType>("MetaField",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" }
                ),
                resolve: context =>
                {
                    if (!metaFieldEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Meta-fields not supported.");
                    }

                    return metaFieldService.GetByParentAsync("category", context.Source.Id,
                        context.GetArgument<string>("name"));
                });
            Field<ListGraphType<MetaFieldGraphType>>("MetaFields",
                resolve: context =>
                {
                    if (!metaFieldEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Meta-fields not supported.");
                    }

                    return metaFieldService.GetByParentAsync("category", context.Source.Id);
                });

            // Products
            Field<ListGraphType<ProductGraphType>>("Products",
                resolve: context =>
                {
                    if (!productEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Products not supported.");
                    }

                    return productService.GetByCategoryIdAsync(context.Source.Id);
                });

            // Category hierarchy
            Field<CategoryGraphType, ICategory>()
                .Name("Parent")
                .ResolveAsync(async ctx =>
                {
                    if (string.IsNullOrEmpty(ctx.Source.ParentId))
                    {
                        return null;
                    }

                    var loader = dataLoaderContextAccessor.Context
                        .GetOrAddCollectionBatchLoader<string, ICategory>("GetByIdAsync", categoryService.LookupByIdAsync);
                    var result = await loader.LoadAsync(ctx.Source.ParentId);
                    return result
                        .FirstOrDefault();
                });
            Field<ListGraphType<CategoryGraphType>>("Children",
                resolve: context => categoryService.GetByParentIdAsync(context.Source.Id));
        }
    }
}
