using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using NodaMoney;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class MoneyGraphType : ObjectGraphType<Money>
    {
        public MoneyGraphType(
            IMetaFieldEndpointProvider metaFieldEndpointProvider,
            IMetaFieldService metaFieldService,
            ICategoryService categoryService
            )
        {
            Name = "Money";

            Field(m => m.Amount);
            Field<StringGraphType>("Currency", resolve: context => context.Source.Currency.Code);
            Field<StringGraphType>("Symbol", resolve: context => context.Source.Currency.Symbol);
            Field<StringGraphType>("IsoNumber", resolve: context => context.Source.Currency.Number);
            Field<StringGraphType>("EnglishName", resolve: context => context.Source.Currency.EnglishName);

            Field<StringGraphType>("DisplayValue", resolve: context => context.Source.ToString("C2"));
        }
    }
}
