using GraphQL;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class NavigationGraphType : ObjectGraphType<INavigation>
    {
        public NavigationGraphType(
            IMetaFieldEndpointProvider metaFieldEndpointProvider,
            IMetaFieldService metaFieldService
        )
        {
            Name = "Navigation";

            Field(m => m.Id);
            Field(m => m.Handle);

            Field(m => m.ParentId, true);


            Field<NavigationLinkGraphType>("Header",
                resolve: context => context.Source.Header
            );
            Field<ListGraphType<NavigationLinkGraphType>>("Links",
                resolve: context => context.Source.Links
            );

            Field<ListGraphType<NavigationGraphType>>("SubNavigations",
                resolve: context => context.Source.SubNavigations
            );

            // Meta-fields
            Field<MetaFieldGraphType>("MetaField",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" }
                ),
                resolve: context =>
                {
                    if (!metaFieldEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Meta-fields not supported.");
                    }

                    return metaFieldService.GetByParentAsync("navigation", context.Source.Id,
                        context.GetArgument<string>("name"));
                });
            Field<ListGraphType<MetaFieldGraphType>>("MetaFields",
                resolve: context =>
                {
                    if (!metaFieldEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Meta-fields not supported.");
                    }

                    return metaFieldService.GetByParentAsync("navigation", context.Source.Id);
                });
        }
    }
}