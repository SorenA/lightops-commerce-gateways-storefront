using System.Collections.Generic;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Providers
{
    public class LanguageProvider : ILanguageProvider
    {
        public string DefaultLanguage { get; internal set; }
        public IList<string> Languages { get; internal set; } = new List<string>();
    }
}