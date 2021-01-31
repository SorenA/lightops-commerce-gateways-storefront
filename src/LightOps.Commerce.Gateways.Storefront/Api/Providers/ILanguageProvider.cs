using System.Collections.Generic;

namespace LightOps.Commerce.Gateways.Storefront.Api.Providers
{
    public interface ILanguageProvider
    {
        string DefaultLanguage { get; }
        IList<string> Languages { get; }
    }
}