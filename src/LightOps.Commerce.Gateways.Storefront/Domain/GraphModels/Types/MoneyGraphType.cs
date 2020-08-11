using GraphQL.Types;
using NodaMoney;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class MoneyGraphType : ObjectGraphType<Money>
    {
        public MoneyGraphType()
        {
            Name = "Money";

            Field<DecimalGraphType, decimal>()
                .Name("Amount")
                .Description("The amount of the currency")
                .Resolve(ctx => ctx.Source.Amount);

            Field<StringGraphType, string>()
                .Name("Currency")
                .Description("The ISO 4217 3-letter code of the currency")
                .Resolve(ctx => ctx.Source.Currency.Code);

            Field<StringGraphType, string>()
                .Name("Symbol")
                .Description("The symbol of the currency")
                .Resolve(ctx => ctx.Source.Currency.Symbol);

            Field<StringGraphType, string>()
                .Name("IsoNumber")
                .Description("The ISO number of the currency")
                .Resolve(ctx => ctx.Source.Currency.Number);

            Field<StringGraphType, string>()
                .Name("EnglishName")
                .Description("The english name of the currency")
                .Resolve(ctx => ctx.Source.Currency.EnglishName);

            Field<StringGraphType, string>()
                .Name("DisplayValue")
                .Description("The value to display for the money with its currency")
                .Resolve(ctx => ctx.Source.ToString("C2"));
        }
    }
}
