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
            INavigationService navigationService
        )
        {
            Name = "Navigation";

            Field(m => m.Id);
            Field(m => m.Handle);

            Field(m => m.ParentId, true);


            Field<NavigationLinkGraphType, INavigationLink>()
                .Name("Header")
                .Resolve(ctx => ctx.Source.Header);
            Field<ListGraphType<NavigationLinkGraphType>, IList<INavigationLink>>()
                .Name("Links")
                .Resolve(ctx => ctx.Source.Links);
            Field<ListGraphType<NavigationGraphType>, IList<INavigation>>()
                .Name("SubNavigations")
                .Resolve(ctx => ctx.Source.SubNavigations);

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

                    return await metaFieldService.GetByParentAsync("navigation", ctx.Source.Id,
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

                    return await metaFieldService.GetByParentAsync("navigation", ctx.Source.Id);
                });

            // Hierarchy
            Field<NavigationGraphType, INavigation>()
                .Name("Parent")
                .ResolveAsync(async ctx =>
                {
                    if (string.IsNullOrEmpty(ctx.Source.ParentId))
                    {
                        return null;
                    }

                    var loader = dataLoaderContextAccessor.Context
                        .GetOrAddCollectionBatchLoader<string, INavigation>("NavigationService.LookupByIdAsync", navigationService.LookupByIdAsync);
                    var result = await loader.LoadAsync(ctx.Source.ParentId);
                    return result
                        .FirstOrDefault();
                });
            Field<ListGraphType<NavigationGraphType>, IList<INavigation>>()
                .Name("Children")
                .ResolveAsync(async ctx => await navigationService.GetByParentIdAsync(ctx.Source.Id));
        }
    }
}