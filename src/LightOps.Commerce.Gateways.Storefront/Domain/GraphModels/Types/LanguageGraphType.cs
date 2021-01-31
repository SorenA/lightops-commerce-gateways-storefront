using GraphQL.Types;
using NodaMoney;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class LanguageGraphType : ObjectGraphType<string>
    {
        public LanguageGraphType()
        {
            Name = "Language";

            Field<StringGraphType, string>()
                .Name("Locale")
                .Description("The ISO 639 2-letter language code matched with ISO 3166 2-letter country code, eg. en-US, da-DK")
                .Resolve(ctx => ctx.Source);
        }
    }
}
