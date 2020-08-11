using System;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using System.Collections.Generic;
using System.Linq;
using NodaMoney;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class ProductGraphType : ObjectGraphType<IProduct>
    {
        public ProductGraphType(
            IDataLoaderContextAccessor dataLoaderContextAccessor,
            IMetaFieldEndpointProvider metaFieldEndpointProvider,
            IMetaFieldService metaFieldService,
            ICategoryLookupService categoryLookupService
            )
        {
            Name = "Product";

            Field<StringGraphType, string>()
                .Name("Id")
                .Description("Globally unique identifier, eg: gid://Product/1000")
                .Resolve(ctx => ctx.Source.Id);

            Field<StringGraphType, string>()
                .Name("ParentId")
                .Description("Globally unique identifier of parent, 'gid://' if none")
                .Resolve(ctx => ctx.Source.ParentId);

            Field<StringGraphType, string>()
                .Name("Handle")
                .Description("A human-friendly unique string for the product")
                .Resolve(ctx => ctx.Source.Handle);

            Field<StringGraphType, string>()
                .Name("Title")
                .Description("The title of the product")
                .Resolve(ctx => ctx.Source.Title);

            Field<StringGraphType, string>()
                .Name("Url")
                .Description("The url of the product")
                .Resolve(ctx => ctx.Source.Url);

            Field<StringGraphType, string>()
                .Name("Type")
                .Description("The type of the product")
                .Resolve(ctx => ctx.Source.Type);

            Field<StringGraphType, string>()
                .Name("Description")
                .Description("The description of the product")
                .Resolve(ctx => ctx.Source.Description);

            Field<DateTimeGraphType, DateTime>()
                .Name("CreatedAt")
                .Description("The timestamp of product creation")
                .Resolve(ctx => ctx.Source.CreatedAt);

            Field<DateTimeGraphType, DateTime>()
                .Name("UpdatedAt")
                .Description("The timestamp of the latest product update")
                .Resolve(ctx => ctx.Source.UpdatedAt);

            Field<StringGraphType, string>()
                .Name("PrimaryCategoryId")
                .Description("Globally unique identifier of the primary category the product belong to")
                .Resolve(ctx => ctx.Source.PrimaryCategoryId);

            Field<ListGraphType<StringGraphType>, IList<string>>()
                .Name("CategoryIds")
                .Description("Globally unique identifiers of categories the product belong to")
                .Resolve(ctx => ctx.Source.CategoryIds);

            Field<ListGraphType<ProductVariantGraphType>, IList<IProductVariant>>()
                .Name("Variants")
                .Description("The variants of the product")
                .Resolve(ctx => ctx.Source.Variants);

            Field<ListGraphType<ImageGraphType>, IList<IImage>>()
                .Name("Images")
                .Description("The images of the product")
                .Resolve(ctx => ctx.Source.Images);

            Field<MoneyGraphType, Money>()
                .Name("UnitPriceFrom")
                .Description("The unit price of the cheapest variant")
                .Resolve(ctx => ctx.Source.Variants.Min(x => x.UnitPrice));

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

                    return await metaFieldService.GetByParentAsync("product", ctx.Source.Id,
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

                    return await metaFieldService.GetByParentAsync("product", ctx.Source.Id);
                });

            // Categories
            Field<CategoryGraphType, ICategory>()
                .Name("PrimaryCategory")
                .ResolveAsync(async ctx =>
                {
                    var loader = dataLoaderContextAccessor.Context
                        .GetOrAddBatchLoader<string, ICategory>("Category.LookupByIdAsync", categoryLookupService.LookupByIdAsync);
                    return await loader.LoadAsync(ctx.Source.PrimaryCategoryId);
                });
            Field<ListGraphType<CategoryGraphType>, IList<ICategory>>()
                .Name("Categories")
                .ResolveAsync(async ctx =>
                {
                    var loader = dataLoaderContextAccessor.Context
                        .GetOrAddBatchLoader<string, ICategory>("Category.LookupByIdAsync", categoryLookupService.LookupByIdAsync);
                    return await loader.LoadAsync(ctx.Source.CategoryIds);
                });
        }
    }
}
