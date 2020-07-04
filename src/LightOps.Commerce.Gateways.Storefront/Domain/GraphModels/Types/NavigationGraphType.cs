using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class NavigationGraphType : ObjectGraphType<INavigation>
    {
        public NavigationGraphType()
        {
            Name = "Navigation";

            Field(m => m.Id);
            Field(m => m.Handle);

            Field(m => m.ParentId, true);

            Field(
                name: "Header",
                type: typeof(NavigationLinkGraphType),
                resolve: context => context.Source.Header
            );
            Field(
                name: "Links",
                type: typeof(ListGraphType<NavigationLinkGraphType>),
                resolve: context => context.Source.Links
            );

            Field(
                name: "SubNavigations",
                type: typeof(ListGraphType<NavigationGraphType>),
                resolve: context => context.Source.SubNavigations
            );
        }
    }
}