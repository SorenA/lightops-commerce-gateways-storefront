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
    public sealed class NavigationGraphType : ObjectGraphType<INavigation>
    {
        public NavigationGraphType(
            IDataLoaderContextAccessor dataLoaderContextAccessor,
            IMetaFieldEndpointProvider metaFieldEndpointProvider,
            IMetaFieldService metaFieldService,
            IMetaFieldLookupService metaFieldLookupService,
            INavigationLookupService navigationLookupService
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
                .Resolve(ctx => ctx.Source.CreatedAt);

            Field<DateTimeGraphType, DateTime>()
                .Name("UpdatedAt")
                .Description("The timestamp of the latest navigation update")
                .Resolve(ctx => ctx.Source.UpdatedAt);

            Field<NavigationLinkGraphType, INavigationLink>()
                .Name("Header")
                .Description("The header link of the navigation")
                .Resolve(ctx => ctx.Source.Header);

            Field<ListGraphType<NavigationLinkGraphType>, IList<INavigationLink>>()
                .Name("Links")
                .Description("The links in the navigation")
                .Resolve(ctx => ctx.Source.Links);

            Field<ListGraphType<SubNavigationGraphType>, IList<ISubNavigation>>()
                .Name("SubNavigations")
                .Description("The embedded sub-navigations")
                .Resolve(ctx => ctx.Source.SubNavigations);

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
        }
    }
}