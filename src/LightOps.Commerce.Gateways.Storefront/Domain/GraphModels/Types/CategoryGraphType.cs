using System;
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
            IProductLookupService productLookupService,
            ICategoryLookupService categoryLookupService
        )
        {
            Name = "Category";

            Field<StringGraphType, string>()
                .Name("Id")
                .Description("Globally unique identifier, eg: gid://Category/1000")
                .Resolve(ctx => ctx.Source.Id);

            Field<StringGraphType, string>()
                .Name("ParentId")
                .Description("Globally unique identifier of parent, 'gid://' if none")
                .Resolve(ctx => ctx.Source.ParentId);

            Field<StringGraphType, string>()
                .Name("Handle")
                .Description("A human-friendly unique string for the category")
                .Resolve(ctx => ctx.Source.Handle);

            Field<StringGraphType, string>()
                .Name("Title")
                .Description("The title of the category")
                .Resolve(ctx => ctx.Source.Title);

            Field<StringGraphType, string>()
                .Name("Url")
                .Description("The url of the category")
                .Resolve(ctx => ctx.Source.Url);

            Field<StringGraphType, string>()
                .Name("Type")
                .Description("The type of the category")
                .Resolve(ctx => ctx.Source.Type);

            Field<StringGraphType, string>()
                .Name("Description")
                .Description("The description of the category")
                .Resolve(ctx => ctx.Source.Description);

            Field<DateTimeGraphType, DateTime>()
                .Name("CreatedAt")
                .Description("The timestamp of category creation")
                .Resolve(ctx => ctx.Source.CreatedAt);

            Field<DateTimeGraphType, DateTime>()
                .Name("UpdatedAt")
                .Description("The timestamp of the latest category update")
                .Resolve(ctx => ctx.Source.UpdatedAt);

            Field<ImageGraphType, IImage>()
                .Name("PrimaryImage")
                .Description("The primary image of the category")
                .Resolve(ctx => ctx.Source.PrimaryImage);

            // Meta-fields
            Field<MetaFieldGraphType, IMetaField>()
                .Name("MetaField")
                .Argument<NonNullGraphType<StringGraphType>>("name")
                .ResolveAsync(async ctx =>
                {
                    if (!metaFieldEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Meta-fields not supported.");
                    }

                    return await metaFieldService.GetByParentAsync("category", ctx.Source.Id,
                        ctx.GetArgument<string>("name"));
                });
            Field<ListGraphType<MetaFieldGraphType>, IList<IMetaField>>()
                .Name("MetaFields")
                .ResolveAsync(async ctx =>
                {
                    if (!metaFieldEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Meta-fields not supported.");
                    }

                    return await metaFieldService.GetByParentAsync("category", ctx.Source.Id);
                });

            // Products
            Field<ListGraphType<ProductGraphType>, IList<IProduct>>()
                .Name("Products")
                .ResolveAsync(async ctx =>
                {
                    if (!productEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Products not supported.");
                    }

                    var loader = dataLoaderContextAccessor.Context
                        .GetOrAddBatchLoader<string, IList<IProduct>>("Product.LookupByCategoryIdAsync", productLookupService.LookupByCategoryIdAsync);
                    return await loader.LoadAsync(ctx.Source.Id);
                });

            // Hierarchy
            Field<CategoryGraphType, ICategory>()
                .Name("Parent")
                .ResolveAsync(async ctx =>
                {
                    if (string.IsNullOrEmpty(ctx.Source.ParentId))
                    {
                        return null;
                    }

                    var loader = dataLoaderContextAccessor.Context
                        .GetOrAddBatchLoader<string, ICategory>("Category.LookupByIdAsync", categoryLookupService.LookupByIdAsync);
                    return await loader.LoadAsync(ctx.Source.ParentId);
                });
            Field<ListGraphType<CategoryGraphType>, IList<ICategory>>()
                .Name("Children")
                .ResolveAsync(async ctx =>
                {
                    var loader = dataLoaderContextAccessor.Context
                        .GetOrAddBatchLoader<string, IList<ICategory>>("Category.LookupByParentIdAsync", categoryLookupService.LookupByParentIdAsync);
                    return await loader.LoadAsync(ctx.Source.Id);
                });
        }
    }
}
