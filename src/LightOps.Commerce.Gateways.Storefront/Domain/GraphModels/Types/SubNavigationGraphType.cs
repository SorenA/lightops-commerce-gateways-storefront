using System.Collections.Generic;
using GraphQL.Types;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class SubNavigationGraphType : ObjectGraphType<SubNavigation>
    {
        public SubNavigationGraphType()
        {
            Name = "SubNavigation";

            Field<NavigationLinkGraphType, NavigationLink>()
                .Name("Header")
                .Description("The header link of the sub-navigation")
                .Resolve(ctx => ctx.Source.Header);

            Field<ListGraphType<NavigationLinkGraphType>, IList<NavigationLink>>()
                .Name("Links")
                .Description("The links in the sub-navigation")
                .Resolve(ctx => ctx.Source.Links);

            Field<ListGraphType<SubNavigationGraphType>, IList<SubNavigation>>()
                .Name("SubNavigations")
                .Description("The embedded sub-sub-navigation")
                .Resolve(ctx => ctx.Source.SubNavigations);

        }
    }
}