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

            Field<StringGraphType, string>()
                .Name("Id")
                .Description("Globally unique identifier, eg: gid://ContentPage/1000")
                .Resolve(ctx => ctx.Source.Id);

            Field<StringGraphType, string>()
                .Name("ParentId")
                .Description("Globally unique identifier of parent, 'gid://' if none")
                .Resolve(ctx => ctx.Source.ParentId);

            Field<StringGraphType, string>()
                .Name("Handle")
                .Description("A human-friendly unique string for the content page")
                .Resolve(ctx => ctx.Source.Handle);

            Field<StringGraphType, string>()
                .Name("Title")
                .Description("The title of the content page")
                .Resolve(ctx => ctx.Source.Title);

            Field<StringGraphType, string>()
                .Name("Url")
                .Description("The url of the content page")
                .Resolve(ctx => ctx.Source.Url);

            Field<StringGraphType, string>()
                .Name("Type")
                .Description("The type of the content page")
                .Resolve(ctx => ctx.Source.Type);

            Field<StringGraphType, string>()
                .Name("Summary")
                .Description("The summary of the content page")
                .Resolve(ctx => ctx.Source.Summary);

            Field<DateTimeGraphType, DateTime>()
                .Name("CreatedAt")
                .Description("The timestamp of content page creation")
                .Resolve(ctx => ctx.Source.CreatedAt);

            Field<DateTimeGraphType, DateTime>()
                .Name("UpdatedAt")
                .Description("The timestamp of the latest content page update")
                .Resolve(ctx => ctx.Source.UpdatedAt);

            Field<ImageGraphType, IImage>()
                .Name("PrimaryImage")
                .Description("The primary image of the content page")
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
