using System.Collections.Generic;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Contexts
{
    public class StorefrontGraphUserContext : Dictionary<string, object>
    {
        public string LanguageCode
        {
            get => this["languageCode"].ToString();
            set => this["languageCode"] = value;
        }

        public string CurrencyCode
        {
            get => this["currencyCode"].ToString();
            set => this["currencyCode"] = value;
        }
    }
}