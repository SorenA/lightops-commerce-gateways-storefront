using LightOps.Mapping.Api.Mappers;
using NodaMoney;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Mappers
{
    public class MoneyProtoMapper : IMapper<Proto.Types.Money, NodaMoney.Money>
    {
        public NodaMoney.Money Map(Proto.Types.Money src)
        {
            var amount = src.Units + src.Nanos / 1_000_000_000;

            return new NodaMoney.Money(amount, Currency.FromCode(src.CurrencyCode));
        }
    }
}