using GraphQL.Types;
using NodaMoney;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class MoneyGraphType : ObjectGraphType<Money>
    {
        public MoneyGraphType()
        {
            Name = "Money";

            Field(m => m.Amount);

            Field<StringGraphType, string>()
                .Name("Currency")
                .Resolve(ctx => ctx.Source.Currency.Code);
            Field<StringGraphType, string>()
                .Name("Symbol")
                .Resolve(ctx => ctx.Source.Currency.Symbol);
            Field<StringGraphType, string>()
                .Name("IsoNumber")
                .Resolve(ctx => ctx.Source.Currency.Number);
            Field<StringGraphType, string>()
                .Name("EnglishName")
                .Resolve(ctx => ctx.Source.Currency.EnglishName);

            Field<StringGraphType, string>()
                .Name("DisplayValue")
                .Resolve(ctx => ctx.Source.ToString("C2"));
        }
    }
}
