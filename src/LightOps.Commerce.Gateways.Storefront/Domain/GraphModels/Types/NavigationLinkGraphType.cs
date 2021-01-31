using System.Linq;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Contexts;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class NavigationLinkGraphType : ObjectGraphType<NavigationLink>
    {
        public NavigationLinkGraphType()
        {
            Name = "NavigationLink";

            Field<StringGraphType, string>()
                .Name("Title")
                .Description("The title of the link")
                .Resolve(ctx =>
                {
                    var userContext = (StorefrontGraphUserContext)ctx.UserContext;

                    return ctx.Source.Titles
                        .FirstOrDefault(x => x.LanguageCode == userContext.LanguageCode)
                        ?.Value;
                });

            Field<StringGraphType, string>()
                .Name("Url")
                .Description("The url of the link, if any")
                .Resolve(ctx =>
                {
                    var userContext = (StorefrontGraphUserContext)ctx.UserContext;

                    return ctx.Source.Urls
                        .FirstOrDefault(x => x.LanguageCode == userContext.LanguageCode)
                        ?.Value;
                });

            Field<StringGraphType, string>()
                .Name("Target")
                .Description("The target of the link, if any")
                .Resolve(ctx => ctx.Source.Target);
        }
    }
}