﻿namespace LightOps.Commerce.Gateways.Storefront.Api.Providers
{
    public interface INavigationServiceProvider
    {
        bool IsEnabled { get; }
        string GrpcEndpoint { get; }
    }
}