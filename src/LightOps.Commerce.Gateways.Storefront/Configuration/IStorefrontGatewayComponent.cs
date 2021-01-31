using System.Collections.Generic;

namespace LightOps.Commerce.Gateways.Storefront.Configuration
{
    public interface IStorefrontGatewayComponent
    {
        /// <summary>
        /// Register languages enabled on the Storefront API.
        /// ISO 639 2-letter language code matched with ISO 3166 2-letter country code, eg. en-US, da-DK
        /// </summary>
        /// <param name="defaultLanguage">The default language, used if a user has no other matching languages</param>
        /// <param name="otherLanguages">The other languages that may be used</param>
        /// <returns></returns>
        IStorefrontGatewayComponent UseLanguages(string defaultLanguage, params string[] otherLanguages);

        /// <summary>
        /// Register currencies enabled on the Storefront API.
        /// ISO 4217 3-letter currency code
        /// </summary>
        /// <param name="defaultCurrency">The default currency, used if a user has no other matching currencies</param>
        /// <param name="otherCurrencies">The other currencies that may be used</param>
        /// <returns></returns>
        IStorefrontGatewayComponent UseCurrencies(string defaultCurrency, params string[] otherCurrencies);

        IStorefrontGatewayComponent UseImageCdn(string cdnHost);

        IStorefrontGatewayComponent UseContentPages(string grpcEndpoint);
        IStorefrontGatewayComponent UseNavigations(string grpcEndpoint);
        IStorefrontGatewayComponent UseMetaFields(string grpcEndpoint);
        IStorefrontGatewayComponent UseCategories(string grpcEndpoint);
        IStorefrontGatewayComponent UseProducts(string grpcEndpoint);
    }
}