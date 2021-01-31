using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GraphQL.Server.Transports.AspNetCore;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using Microsoft.AspNetCore.Http;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Contexts
{
    public class StorefrontGraphUserContextBuilder : IUserContextBuilder
    {
        private readonly ILanguageProvider _languageProvider;
        private readonly ICurrencyProvider _currencyProvider;

        public StorefrontGraphUserContextBuilder(
            ILanguageProvider languageProvider,
            ICurrencyProvider currencyProvider)
        {
            _languageProvider = languageProvider;
            _currencyProvider = currencyProvider;
        }

        public Task<IDictionary<string, object>> BuildUserContext(HttpContext httpContext)
        {
            // Get first matching language from request header
            var language = httpContext.Request.GetTypedHeaders()
                .AcceptLanguage
                ?.OrderByDescending(x => x.Quality ?? 1)
                .Select(x => x.Value.ToString())
                .FirstOrDefault(x => _languageProvider.Languages.Contains(x));

            // Get matching currency

            var currency = httpContext.Request.Headers.TryGetValue("X-Currency", out var currencyHeaderValue)
                           && _currencyProvider.Currencies.Contains(currencyHeaderValue.ToString())
                ? currencyHeaderValue.ToString()
                : null;

            return Task.FromResult<IDictionary<string, object>>(new StorefrontGraphUserContext
            {
                LanguageCode = language ?? _languageProvider.DefaultLanguage,
                CurrencyCode = currency ?? _currencyProvider.DefaultCurrency,
            });
        }
    }
}
