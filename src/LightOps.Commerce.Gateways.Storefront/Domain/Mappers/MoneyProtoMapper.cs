using LightOps.Commerce.Proto.Types;
using LightOps.Mapping.Api.Mappers;
using NodaMoney;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Mappers
{
    public class MoneyProtoMapper : IMapper<MoneyProto, Money>
    {
        public Money Map(MoneyProto src)
        {
            var amount = src.Units + src.Nanos / 1_000_000_000;

            return new Money(amount, Currency.FromCode(src.CurrencyCode));
        }
    }
}