using LightOps.Commerce.Proto.Services.Product.V1;
using LightOps.Mapping.Api.Mappers;
using NodaMoney;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Mappers.V1
{
    public class ProtoMoneyMapper : IMapper<ProtoMoney, Money>
    {
        public Money Map(ProtoMoney source)
        {
            var amount = source.Units + source.Nanos / 1_000_000_000;

            return new Money(amount, Currency.FromCode(source.CurrencyCode));
        }
    }
}