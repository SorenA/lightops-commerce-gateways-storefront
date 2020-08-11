using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Enums;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Enum;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class CategoryGraphType : ObjectGraphType<ICategory>
    {
        public CategoryGraphType(
            IDataLoaderContextAccessor dataLoaderContextAccessor,
            IMetaFieldEndpointProvider metaFieldEndpointProvider,
            IMetaFieldService metaFieldService,
            IMetaFieldLookupService metaFieldLookupService,
            IProductEndpointProvider productEndpointProvider,
            IProductService productService,
            ICategoryService categoryService,
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

            #region Meta-fields

            Field<MetaFieldGraphType, IMetaField>()
                .Name("MetaField")
                .Description("Connection to a specific meta-field")
                .Argument<NonNullGraphType<StringGraphType>>("namespace", "Namespace of the meta-field")
                .Argument<NonNullGraphType<StringGraphType>>("name", "Name of the meta-field")
                .ResolveAsync(async ctx =>
                {
                    if (!metaFieldEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Meta-fields not supported.");
                    }

                    var result = await metaFieldService.GetBySearchAsync(
                        ctx.Source.Id,
                        ctx.GetArgument<string>("namespace"),
                        ctx.GetArgument<string>("name"));

                    return result.FirstOrDefault();
                });

            Field<ListGraphType<MetaFieldGraphType>, IList<IMetaField>>()
                .Name("MetaFields")
                .Description("Connection to a all meta-fields")
                .ResolveAsync(async ctx =>
                {
                    if (!metaFieldEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Meta-fields not supported.");
                    }

                    var loader = dataLoaderContextAccessor.Context
                        .GetOrAddBatchLoader<string, IList<IMetaField>>("MetaField.LookupByParentIdsAsync", metaFieldLookupService.LookupByParentIdsAsync);
                    return await loader.LoadAsync(ctx.Source.Id);
                });

            #endregion

            #region Categories

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

            Connection<CategoryGraphType>()
                .Name("Children")
                .Unidirectional()
                .Argument<StringGraphType>("query", "The search query to filter children by")
                .Argument<CategorySortKeyGraphType>("sortKey", "The key to sort the underlying list by")
                .Argument<StringGraphType>("reverse", "Reverse the order of the underlying list")
                .ResolveAsync(async ctx =>
                {
                    var result = await categoryService.GetBySearchAsync(
                        ctx.GetArgument<string>("query"),
                        ctx.Source.Id,
                        ctx.GetArgument<string>("after"),
                        ctx.GetArgument<int>("first", 24),
                        ctx.GetArgument<CategorySortKey>("sortKey"),
                        ctx.GetArgument<bool>("reverse"));

                    return result.ToGraphConnection();
                });

            #endregion Categories

            #region Products

            Connection<ProductGraphType>()
                .Name("Products")
                .Unidirectional()
                .Argument<StringGraphType>("query", "The search query to filter products by")
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
                        ctx.Source.Id,
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
