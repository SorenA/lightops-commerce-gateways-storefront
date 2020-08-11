using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class NavigationLinkGraphType : ObjectGraphType<INavigationLink>
    {
        public NavigationLinkGraphType()
        {
            Name = "NavigationLink";

            Field<StringGraphType, string>()
                .Name("Title")
                .Description("The title of the link")
                .Resolve(ctx => ctx.Source.Title);

            Field<StringGraphType, string>()
                .Name("Url")
                .Description("The url of the link, if any")
                .Resolve(ctx => ctx.Source.Url);

            Field<StringGraphType, string>()
                .Name("Target")
                .Description("The target of the link, if any")
                .Resolve(ctx => ctx.Source.Target);
        }
    }
}