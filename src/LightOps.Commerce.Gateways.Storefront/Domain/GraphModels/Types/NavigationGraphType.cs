using System;
using System.Collections.Generic;
using System.Linq;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class NavigationGraphType : ObjectGraphType<Navigation>
    {
        public NavigationGraphType(
            IDataLoaderContextAccessor dataLoaderContextAccessor,
            IMetaFieldServiceProvider metaFieldServiceProvider,
            IMetaFieldService metaFieldService,
            IMetaFieldLookupService metaFieldLookupService
        )
        {
            Name = "Navigation";

            Field<StringGraphType, string>()
                .Name("Id")
                .Description("Globally unique identifier, eg: gid://Navigation/1000")
                .Resolve(ctx => ctx.Source.Id);

            Field<StringGraphType, string>()
                .Name("ParentId")
                .Description("Globally unique identifier of parent, 'gid://' if none")
                .Resolve(ctx => ctx.Source.ParentId);

            Field<StringGraphType, string>()
                .Name("Handle")
                .Description("A human-friendly unique string for the navigation")
                .Resolve(ctx => ctx.Source.Handle);

            Field<StringGraphType, string>()
                .Name("Type")
                .Description("The type of the navigation")
                .Resolve(ctx => ctx.Source.Type);

            Field<DateTimeGraphType, DateTime>()
                .Name("CreatedAt")
                .Description("The timestamp of navigation creation")
                .Resolve(ctx => ctx.Source.CreatedAt.ToDateTime());

            Field<DateTimeGraphType, DateTime>()
                .Name("UpdatedAt")
                .Description("The timestamp of the latest navigation update")
                .Resolve(ctx => ctx.Source.UpdatedAt.ToDateTime());

            Field<NavigationLinkGraphType, NavigationLink>()
                .Name("Header")
                .Description("The header link of the navigation")
                .Resolve(ctx => ctx.Source.Header);

            Field<ListGraphType<NavigationLinkGraphType>, IList<NavigationLink>>()
                .Name("Links")
                .Description("The links in the navigation")
                .Resolve(ctx => ctx.Source.Links);

            Field<ListGraphType<SubNavigationGraphType>, IList<SubNavigation>>()
                .Name("SubNavigations")
                .Description("The embedded sub-navigations")
                .Resolve(ctx => ctx.Source.SubNavigations);

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
        }
    }
}