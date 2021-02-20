using System;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using System.Collections.Generic;
using System.Linq;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Contexts;
using LightOps.Commerce.Proto.Types;
using LightOps.Mapping.Api.Services;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class ProductGraphType : ObjectGraphType<Product>
    {
        public ProductGraphType(
            IDataLoaderContextAccessor dataLoaderContextAccessor,
            IMetaFieldServiceProvider metaFieldServiceProvider,
            IMetaFieldService metaFieldService,
            IMetaFieldLookupService metaFieldLookupService,
            ICategoryServiceProvider categoryServiceProvider,
            ICategoryLookupService categoryLookupService,
            IMappingService mappingService
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
                .Resolve(ctx =>
                {
                    var userContext = (StorefrontGraphUserContext)ctx.UserContext;

                    return ctx.Source.Handles
                        .FirstOrDefault(x => x.LanguageCode == userContext.LanguageCode)
                        ?.Value;
                });

            Field<StringGraphType, string>()
                .Name("Title")
                .Description("The title of the product")
                .Resolve(ctx =>
                {
                    var userContext = (StorefrontGraphUserContext)ctx.UserContext;

                    return ctx.Source.Titles
                        .FirstOrDefault(x => x.LanguageCode == userContext.LanguageCode)
                        ?.Value;
                });

            Field<StringGraphType, string>()
                .Name("Url")
                .Description("The url of the product")
                .Resolve(ctx =>
                {
                    var userContext = (StorefrontGraphUserContext)ctx.UserContext;

                    return ctx.Source.Urls
                        .FirstOrDefault(x => x.LanguageCode == userContext.LanguageCode)
                        ?.Value;
                });

            Field<StringGraphType, string>()
                .Name("Type")
                .Description("The type of the product")
                .Resolve(ctx => ctx.Source.Type);

            Field<StringGraphType, string>()
                .Name("Description")
                .Description("The description of the product")
                .Resolve(ctx =>
                {
                    var userContext = (StorefrontGraphUserContext)ctx.UserContext;

                    return ctx.Source.Descriptions
                        .FirstOrDefault(x => x.LanguageCode == userContext.LanguageCode)
                        ?.Value;
                });

            Field<DateTimeGraphType, DateTime>()
                .Name("CreatedAt")
                .Description("The timestamp of product creation")
                .Resolve(ctx => ctx.Source.CreatedAt.ToDateTime());

            Field<DateTimeGraphType, DateTime>()
                .Name("UpdatedAt")
                .Description("The timestamp of the latest product update")
                .Resolve(ctx => ctx.Source.UpdatedAt.ToDateTime());

            Field<StringGraphType, string>()
                .Name("PrimaryCategoryId")
                .Description("Globally unique identifier of the primary category the product belong to")
                .Resolve(ctx => ctx.Source.PrimaryCategoryId);

            Field<ListGraphType<StringGraphType>, IList<string>>()
                .Name("CategoryIds")
                .Description("Globally unique identifiers of categories the product belong to")
                .Resolve(ctx => ctx.Source.CategoryIds);

            Field<ListGraphType<ProductVariantGraphType>, IList<ProductVariant>>()
                .Name("Variants")
                .Description("The variants of the product")
                .Resolve(ctx => ctx.Source.Variants
                    .OrderBy(x => x.SortOrder)
                    .ToList());

            Field<ListGraphType<ImageGraphType>, IList<Image>>()
                .Name("Images")
                .Description("The images of the product")
                .Resolve(ctx => ctx.Source.Images);

            Field<ImageGraphType, Image>()
                .Name("PrimaryImage")
                .Description("The primary image of the product")
                .Resolve(ctx =>
                {
                    if (ctx.Source.Images.Any())
                    {
                        // First image of product
                        return ctx.Source.Images.First();
                    }

                    foreach (var productVariant in ctx.Source.Variants)
                    {
                        if (productVariant.Images.Any())
                        {
                            // First image of product product
                            return ctx.Source.Images.First();
                        }
                    }

                    // No images
                    return null;
                });

            Field<MoneyGraphType, NodaMoney.Money>()
                .Name("UnitPriceFrom")
                .Description("The unit price of the cheapest variant")
                .Resolve(ctx =>
                {
                    var userContext = (StorefrontGraphUserContext) ctx.UserContext;

                    // Join unit prices for currency code and map to NodaMoney, return smallest
                    return mappingService
                        .Map<Proto.Types.Money, NodaMoney.Money>(
                            ctx.Source.Variants
                                .SelectMany(x =>
                                    x.UnitPrices.Where(p => p.CurrencyCode == userContext.CurrencyCode)))
                        .Min();
                });
            #region Meta-fields

            Field<MetaFieldGraphType, MetaField>()
                .Name("MetaField")
                .Description("Connection to a specific meta-field")
                .Argument<NonNullGraphType<StringGraphType>>("namespace", "Namespace of the meta-field")
                .Argument<NonNullGraphType<StringGraphType>>("name", "Name of the meta-field")
                .ResolveAsync(async ctx =>
                {
                    if (!metaFieldServiceProvider.IsEnabled)
                    {
                        throw new ExecutionError("Meta-fields not supported.");
                    }

                    var result = await metaFieldService.GetBySearchAsync(
                        ctx.Source.Id,
                        ctx.GetArgument<string>("namespace"),
                        ctx.GetArgument<string>("name"));

                    return result.FirstOrDefault();
                });

            Field<ListGraphType<MetaFieldGraphType>, IList<MetaField>>()
                .Name("MetaFields")
                .Description("Connection to a all meta-fields")
                .ResolveAsync(ctx =>
                {
                    if (!metaFieldServiceProvider.IsEnabled)
                    {
                        throw new ExecutionError("Meta-fields not supported.");
                    }

                    var loader = dataLoaderContextAccessor.Context
                        .GetOrAddBatchLoader<string, IList<MetaField>>("MetaField.LookupByParentIdsAsync", metaFieldLookupService.LookupByParentIdsAsync);
                    return loader.LoadAsync(ctx.Source.Id);
                });

            #endregion Meta-fields

            #region Categories

            Field<CategoryGraphType, Category>()
                .Name("PrimaryCategory")
                .ResolveAsync(ctx =>
                {
                    if (!categoryServiceProvider.IsEnabled)
                    {
                        throw new ExecutionError("Categories not supported.");
                    }

                    var loader = dataLoaderContextAccessor.Context
                        .GetOrAddBatchLoader<string, Category>("Category.LookupByIdAsync", categoryLookupService.LookupByIdAsync);
                    return loader.LoadAsync(ctx.Source.PrimaryCategoryId);
                });

            Field<ListGraphType<CategoryGraphType>, Category[]>()
                .Name("Categories")
                .ResolveAsync(ctx =>
                {
                    if (!categoryServiceProvider.IsEnabled)
                    {
                        throw new ExecutionError("Categories not supported.");
                    }

                    var loader = dataLoaderContextAccessor.Context
                        .GetOrAddBatchLoader<string, Category>("Category.LookupByIdAsync", categoryLookupService.LookupByIdAsync);
                    return loader.LoadAsync(ctx.Source.CategoryIds);
                });

            #endregion Categories
        }
    }
}
