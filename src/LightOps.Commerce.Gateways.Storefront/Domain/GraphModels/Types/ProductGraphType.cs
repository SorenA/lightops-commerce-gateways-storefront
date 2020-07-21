﻿using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using System.Collections.Generic;

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

            Field(m => m.Id);
            Field(m => m.Handle);
            Field(m => m.Url);

            Field(m => m.Title);
            Field(m => m.Type);
            Field(m => m.Description);

            Field(m => m.SeoTitle);
            Field(m => m.SeoDescription);

            Field(m => m.PrimaryCategoryId);
            Field(m => m.CategoryIds);

            Field<ListGraphType<ProductVariantGraphType>, IList<IProductVariant>>()
                .Name("Variants")
                .Resolve(ctx => ctx.Source.Variants);

            Field(m => m.PrimaryImage);
            Field(m => m.Images);

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
