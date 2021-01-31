using System.Collections.Generic;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Providers
{
    public class CurrencyProvider : ICurrencyProvider
    {
        public string DefaultCurrency { get; internal set; }
        public IList<string> Currencies { get; internal set; } = new List<string>();
    }
}