using System.Collections.Generic;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class SubNavigationGraphType : ObjectGraphType<ISubNavigation>
    {
        public SubNavigationGraphType()
        {
            Name = "SubNavigation";

            Field<NavigationLinkGraphType, INavigationLink>()
                .Name("Header")
                .Description("The header link of the sub-navigation")
                .Resolve(ctx => ctx.Source.Header);

            Field<ListGraphType<NavigationLinkGraphType>, IList<INavigationLink>>()
                .Name("Links")
                .Description("The links in the sub-navigation")
                .Resolve(ctx => ctx.Source.Links);

            Field<ListGraphType<SubNavigationGraphType>, IList<ISubNavigation>>()
                .Name("SubNavigations")
                .Description("The embedded sub-sub-navigation")
                .Resolve(ctx => ctx.Source.SubNavigations);

        }
    }
}