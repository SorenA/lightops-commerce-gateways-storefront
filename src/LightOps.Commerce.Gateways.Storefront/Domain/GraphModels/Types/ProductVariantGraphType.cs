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
            IDataLoaderContextAccessor dataLoaderContextAccessor,
            IMetaFieldEndpointProvider metaFieldEndpointProvider,
            IMetaFieldService metaFieldService,
            IMetaFieldLookupService metaFieldLookupService
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

            Field<ImageGraphType, IImage>()
                .Name("PrimaryImage")
                .Description("The primary image of the product variant")
                .Resolve(ctx => ctx.Source.Images.FirstOrDefault());

            #region Meta-fields

            Field<MetaFieldGraphType, IMetaField>()
                .Name("MetaField")
                .Description("Connection to a specific meta-field")
                .Argument<NonNullGraphType<StringGraphType>>("namespace", "Namespace of the meta-field")
                .Argument<NonNullGraphType<StringGraphType>>("name", "Name of the meta-field")
                .ResolveAsync(async ctx =>
                {
                    if (!metaFieldEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Meta-fields not supported.");
                    }

                    var result = await metaFieldService.GetBySearchAsync(
                        ctx.Source.Id,
                        ctx.GetArgument<string>("namespace"),
                        ctx.GetArgument<string>("name"));

                    return result.FirstOrDefault();
                });

            Field<ListGraphType<MetaFieldGraphType>, IList<IMetaField>>()
                .Name("MetaFields")
                .Description("Connection to a all meta-fields")
                .ResolveAsync(async ctx =>
                {
                    if (!metaFieldEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Meta-fields not supported.");
                    }

                    var loader = dataLoaderContextAccessor.Context
                        .GetOrAddBatchLoader<string, IList<IMetaField>>("MetaField.LookupByParentIdsAsync", metaFieldLookupService.LookupByParentIdsAsync);
                    return await loader.LoadAsync(ctx.Source.Id);
                });

            #endregion Meta-fields
        }
    }
}