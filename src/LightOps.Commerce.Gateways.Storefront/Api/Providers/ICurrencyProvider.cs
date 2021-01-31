using System.Collections.Generic;

namespace LightOps.Commerce.Gateways.Storefront.Api.Providers
{
    public interface ICurrencyProvider
    {
        string DefaultCurrency{ get; }
        IList<string> Currencies { get; }
    }
}