using GraphQL;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class ProductVariantGraphType : ObjectGraphType<IProductVariant>
    {
        public ProductVariantGraphType(
            IMetaFieldEndpointProvider metaFieldEndpointProvider,
            IMetaFieldService metaFieldService
            )
        {
            Name = "ProductVariant";

            Field(m => m.Id);

            Field(m => m.ProductId);

            Field(m => m.Title);
            Field(m => m.Sku);

            Field<MoneyGraphType>("Price", resolve: context => context.Source.Price);

            // Meta-fields
            Field<MetaFieldGraphType>("MetaField",
                arguments: new QueryArguments(
                    new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "name" }
                ),
                resolve: context =>
                {
                    if (!metaFieldEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Meta-fields not supported.");
                    }

                    return metaFieldService.GetByParentAsync("product_variant", context.Source.Id,
                        context.GetArgument<string>("name"));
                });
            Field<ListGraphType<MetaFieldGraphType>>("MetaFields",
                resolve: context =>
                {
                    if (!metaFieldEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Meta-fields not supported.");
                    }

                    return metaFieldService.GetByParentAsync("product_variant", context.Source.Id);
                });
        }
    }
}
