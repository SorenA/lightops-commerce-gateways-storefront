using System;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Queries;
using Microsoft.Extensions.DependencyInjection;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Schemas
{
    public class StorefrontGraphSchema : Schema
    {
        public StorefrontGraphSchema(IServiceProvider serviceProvider)
            : base(serviceProvider)
        {
            Query = serviceProvider.GetService<StorefrontGraphQuery>();
        }
    }
}