﻿using System.Collections.Generic;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class ContentPageGraphType : ObjectGraphType<IContentPage>
    {
        public ContentPageGraphType(
            IDataLoaderContextAccessor dataLoaderContextAccessor,
            IMetaFieldEndpointProvider metaFieldEndpointProvider,
            IMetaFieldService metaFieldService,
            IContentPageLookupService contentPageLookupService
            )
        {
            Name = "ContentPage";

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
            Field<MetaFieldGraphType, IMetaField>()
                .Name("MetaField")
                .Argument<NonNullGraphType<StringGraphType>>("name")
                .ResolveAsync(async ctx =>
                {
                    if (!metaFieldEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Meta-fields not supported.");
                    }

                    return await metaFieldService.GetByParentAsync("content_page", ctx.Source.Id,
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

                    return await metaFieldService.GetByParentAsync("content_page", ctx.Source.Id);
                });

            // Hierarchy
            Field<ContentPageGraphType, IContentPage>()
                .Name("Parent")
                .ResolveAsync(async ctx =>
                {
                    if (string.IsNullOrEmpty(ctx.Source.ParentId))
                    {
                        return null;
                    }

                    var loader = dataLoaderContextAccessor.Context
                        .GetOrAddBatchLoader<string, IContentPage>("ContentPage.LookupByIdAsync", contentPageLookupService.LookupByIdAsync);
                    return await loader.LoadAsync(ctx.Source.ParentId);
                });
            Field<ListGraphType<ContentPageGraphType>, IList<IContentPage>>()
                .Name("Children")
                .ResolveAsync(async ctx =>
                {
                    var loader = dataLoaderContextAccessor.Context
                        .GetOrAddBatchLoader<string, IList<IContentPage>>("ContentPage.LookupByParentIdAsync", contentPageLookupService.LookupByParentIdAsync);
                    return await loader.LoadAsync(ctx.Source.Id);
                });
        }
    }
}
