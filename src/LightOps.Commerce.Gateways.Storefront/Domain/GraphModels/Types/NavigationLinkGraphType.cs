using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class NavigationLinkGraphType : ObjectGraphType<INavigationLink>
    {
        public NavigationLinkGraphType()
        {
            Name = "NavigationLink";

            Field(m => m.Title);
            Field(m => m.Url);
            Field(m => m.Target);
        }
    }
}