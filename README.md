# LightOps Commerce - Storefront API Gateway

GraphQL based Storefront API gateway for LightOps Commerce services.

Provides a GraphQL API for the following services:

- Content Page
- Navigation
- MetaField
- Category
- Product

![Nuget](https://img.shields.io/nuget/v/LightOps.Commerce.Gateways.Storefront)

| Branch | CI |
| --- | --- |
| master | ![Build Status](https://dev.azure.com/sorendev/LightOps%20Commerce/_apis/build/status/LightOps.Commerce.Gateways.Storefront?branchName=master) |
| develop | ![Build Status](https://dev.azure.com/sorendev/LightOps%20Commerce/_apis/build/status/LightOps.Commerce.Gateways.Storefront?branchName=develop) |

## Samples

A sample application hosting the GraphQL API is available in the `samples/Sample.StorefrontGateway` project.

Sample applications hosting the gRPC services with mock data are also available:

- `samples/Sample.ContentPageService`
- `samples/Sample.NavigationService`
- `samples/Sample.MetaFieldService`
- `samples/Sample.CategoryService`
- `samples/Sample.ProductService`

All samples may be started simultaneously using the `docker-compose` setup in the  `samples/Sample.DockerCompose` project.

## Requirements

LightOps packages available on NuGet:

- `LightOps.DependencyInjection`
- `LightOps.Mapping`

## Using the gateway component

Register during startup through the `AddStorefrontGateway(options)` extension on `IDependencyInjectionRootComponent`. (Using app settings in a non-sample setup of course)

```csharp
services.AddLightOpsDependencyInjection(root =>
{
    root
        .AddMapping()
        .AddCqrs()
        .AddStorefrontGateway(gateway =>
        {
            // Configure languages
            gateway.UseLanguages("en-US", "da-DK");

            // Configure currencies
            gateway.UseCurrencies("EUR", "DKK");

            // Configure CDN
            gateway.UseImageCdn("https://cdn.example.com");

            // Configure service connections
            gateway.UseContentPages("http://sample-content-page-service:80");
            gateway.UseNavigations("http://sample-navigation-service:80");
            gateway.UseMetaFields("http://sample-meta-field-service:80");
            gateway.UseCategories("http://sample-category-service:80");
            gateway.UseProducts("http://sample-product-service:80");

            // Configure GraphQL
            gateway.ConfigureGraphQL((options, provider) =>
            {
                options.EnableMetrics = true;
                // ...
            });
        });
});
```

Enable GraphQL

```csharp
// Use HTTP middleware at path /graphql
app.UseGraphQL<ISchema>();
```

Localization is managed using the header `Accept-Language`, with the first weighted match being selected.
If no header is provided or the value isn't in the enabled language list, it will fallback to the default language.

Currency selection is managed using the header `X-Currency`.
If no header is provided or the value isn't in the enabled currency list, it will fallback to the default currency.

### Configuration options

Using the `IStorefrontGatewayComponent` configuration, the following can be configured:

```csharp
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
```

`UseImageCdn` will enable CDN prefixing, by prefixing the defined host to images without an absolute uri.
