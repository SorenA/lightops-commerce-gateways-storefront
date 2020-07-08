# LightOps Commerce - Storefront API Gateway

GraphQL based Storefront API gateway for LightOps Commerce services.

Provides a GraphQL API for the following services:

- Content Page V1
  - Nested MetaField V1
- Navigation V1
  - Nested MetaField V1

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

All samples may be started simultaneously using the `docker-compose` setup in the  `samples/Sample.DockerCompose` project.

## Requirements

LightOps packages available on NuGet:

- `LightOps.DependencyInjection`
- `LightOps.Mapping`

## Using the gateway component

Register during startup through the `AddStorefrontGateway(options)` extension on `IDependencyInjectionRootComponent`.

```csharp
services.AddLightOpsDependencyInjection(root =>
{
    root
        .AddMapping()
        .AddCqrs()
        .AddStorefrontGateway(gateway =>
        {
            // Configure service connections
            gateway.UseContentPages("http://sample-content-page-service:80");
            gateway.UseNavigations("http://sample-navigation-service:80");
            gateway.UseMetaFields("http://sample-meta-field-service:80");
        });
});
```

Register GraphQL

```csharp
services
    .AddGraphQL((options, provider) =>
    {
        // Configure GraphQL
        // ...
    })
    .AddSystemTextJson(deserializerSettings => { }, serializerSettings => { });
```

Enable GraphQL

```csharp
// Use HTTP middleware at path /graphql
app.UseGraphQL<ISchema>();
```

### Configuration options

Using the `IStorefrontGatewayComponent` configuration, the following can be configured:

```csharp
public interface IStorefrontGatewayComponent
{
    IStorefrontGatewayComponent UseContentPages(string grpcEndpoint);
    IStorefrontGatewayComponent UseNavigations(string grpcEndpoint);
    IStorefrontGatewayComponent UseMetaFields(string grpcEndpoint);
}
```
