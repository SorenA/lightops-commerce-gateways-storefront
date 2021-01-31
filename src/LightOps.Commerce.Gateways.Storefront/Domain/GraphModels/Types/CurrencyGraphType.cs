using GraphQL.Types;
using NodaMoney;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class CurrencyGraphType : ObjectGraphType<Currency>
    {
        public CurrencyGraphType()
        {
            Name = "Currency";

            Field<StringGraphType, string>()
                .Name("Currency")
                .Description("The ISO 4217 3-letter code of the currency")
                .Resolve(ctx => ctx.Source.Code);

            Field<StringGraphType, string>()
                .Name("Symbol")
                .Description("The symbol of the currency")
                .Resolve(ctx => ctx.Source.Symbol);

            Field<StringGraphType, string>()
                .Name("IsoNumber")
                .Description("The ISO number of the currency")
                .Resolve(ctx => ctx.Source.Number);

            Field<StringGraphType, string>()
                .Name("EnglishName")
                .Description("The english name of the currency")
                .Resolve(ctx => ctx.Source.EnglishName);
        }
    }
}
