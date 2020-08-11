using System.Collections.Generic;
using System.Linq;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using NodaMoney;

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

            Field<StringGraphType, string>()
                .Name("Id")
                .Description("Globally unique identifier, eg: gid://ProductVariant/1000")
                .Resolve(ctx => ctx.Source.Id);

            Field<StringGraphType, string>()
                .Name("ParentId")
                .Description("Globally unique identifier of the parent product")
                .Resolve(ctx => ctx.Source.ProductId);

            Field<StringGraphType, string>()
                .Name("Title")
                .Description("The title of the product variant")
                .Resolve(ctx => ctx.Source.Title);

            Field<StringGraphType, string>()
                .Name("Sku")
                .Description("The stock keeping unit of the product variant")
                .Resolve(ctx => ctx.Source.Sku);

            Field<MoneyGraphType, Money>()
                .Name("UnitPrice")
                .Description("The unit price of the product variant")
                .Resolve(ctx => ctx.Source.UnitPrice);

            Field<ListGraphType<ImageGraphType>, IList<IImage>>()
                .Name("Images")
                .Description("The images of the product variant")
                .Resolve(ctx => ctx.Source.Images);

            // Meta-fields
            Field<MetaFieldGraphType, IMetaField>()
                .Name("MetaField")
                .Argument<NonNullGraphType<StringGraphType>>("name")
                .ResolveAsync(async ctx =>
                {
                    if (!metaFieldEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Meta-fields not supported.");
                    }

                    return await metaFieldService.GetByParentAsync("product_variant", ctx.Source.Id,
                        ctx.GetArgument<string>("name"));
                });
            Field<ListGraphType<MetaFieldGraphType>, IList<IMetaField>>()
                .Name("MetaFields")
                .ResolveAsync(async ctx =>
                {
                    if (!metaFieldEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Meta-fields not supported.");
                    }

                    return await metaFieldService.GetByParentAsync("product_variant", ctx.Source.Id);
                });
        }
    }
}
