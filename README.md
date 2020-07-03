# LightOps Commerce - Storefront API Gateway

GraphQL based Storefront API gateway for LightOps Commerce services.

## HTTP services

To be implemented.

### Health-check

To be implemented.

## Samples

To be implemented.

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
            // Configure gateway
            // ...
        });
});
```
