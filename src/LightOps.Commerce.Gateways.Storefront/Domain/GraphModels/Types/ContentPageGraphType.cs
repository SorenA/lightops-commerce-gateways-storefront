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
    public sealed class ContentPageGraphType : ObjectGraphType<IContentPage>
    {
        public ContentPageGraphType(
            IDataLoaderContextAccessor dataLoaderContextAccessor,
            IMetaFieldEndpointProvider metaFieldEndpointProvider,
            IMetaFieldService metaFieldService,
            IMetaFieldLookupService metaFieldLookupService,
            IContentPageService contentPageService,
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

            #endregion Meta-fields

            #region Content pages

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
            Connection<ContentPageGraphType>()
                .Name("Children")
                .Unidirectional()
                .Argument<StringGraphType>("query", "The search query to filter children by")
                .Argument<ContentPageSortKeyGraphType>("sortKey", "The key to sort the underlying list by")
                .Argument<StringGraphType>("reverse", "Reverse the order of the underlying list")
                .ResolveAsync(async ctx =>
                {
                    var result = await contentPageService.GetBySearchAsync(
                        ctx.GetArgument<string>("query"),
                        ctx.Source.Id,
                        ctx.GetArgument<string>("after"),
                        ctx.GetArgument<int>("first", 24),
                        ctx.GetArgument<ContentPageSortKey>("sortKey"),
                        ctx.GetArgument<bool>("reverse"));

                    return result.ToGraphConnection();
                });

            #endregion Content pages
        }
    }
}