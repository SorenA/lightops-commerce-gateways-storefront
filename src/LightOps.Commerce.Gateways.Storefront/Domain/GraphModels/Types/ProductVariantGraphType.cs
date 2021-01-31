using System.Collections.Generic;
using System.Linq;
using GraphQL;
using GraphQL.DataLoader;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Contexts;
using LightOps.Commerce.Proto.Types;
using LightOps.Mapping.Api.Services;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class ProductVariantGraphType : ObjectGraphType<ProductVariant>
    {
        public ProductVariantGraphType(
            IDataLoaderContextAccessor dataLoaderContextAccessor,
            IMetaFieldServiceProvider metaFieldServiceProvider,
            IMetaFieldService metaFieldService,
            IMetaFieldLookupService metaFieldLookupService,
            IMappingService mappingService
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
                .Resolve(ctx =>
                {
                    var userContext = (StorefrontGraphUserContext)ctx.UserContext;

                    return ctx.Source.Titles
                        .FirstOrDefault(x => x.LanguageCode == userContext.LanguageCode)
                        ?.Value;
                });

            Field<StringGraphType, string>()
                .Name("Sku")
                .Description("The stock keeping unit of the product variant")
                .Resolve(ctx => ctx.Source.Sku);

            Field<MoneyGraphType, NodaMoney.Money>()
                .Name("UnitPrice")
                .Description("The unit price of the product variant")
                .Resolve(ctx =>
                {
                    var userContext = (StorefrontGraphUserContext)ctx.UserContext;

                    return mappingService
                        .Map<Proto.Types.Money, NodaMoney.Money>(
                            ctx.Source.UnitPrices.FirstOrDefault(p => p.CurrencyCode == userContext.CurrencyCode));
                });

            Field<ListGraphType<ImageGraphType>, IList<Image>>()
                .Name("Images")
                .Description("The images of the product variant")
                .Resolve(ctx => ctx.Source.Images);

            Field<ImageGraphType, Image>()
                .Name("PrimaryImage")
                .Description("The primary image of the product variant")
                .Resolve(ctx => ctx.Source.Images.FirstOrDefault());

            #region Meta-fields

            Field<MetaFieldGraphType, MetaField>()
                .Name("MetaField")
                .Description("Connection to a specific meta-field")
                .Argument<NonNullGraphType<StringGraphType>>("namespace", "Namespace of the meta-field")
                .Argument<NonNullGraphType<StringGraphType>>("name", "Name of the meta-field")
                .ResolveAsync(async ctx =>
                {
                    if (!metaFieldServiceProvider.IsEnabled)
                    {
                        throw new ExecutionError("Meta-fields not supported.");
                    }

                    var result = await metaFieldService.GetBySearchAsync(
                        ctx.Source.Id,
                        ctx.GetArgument<string>("namespace"),
                        ctx.GetArgument<string>("name"));

                    return result.FirstOrDefault();
                });

            Field<ListGraphType<MetaFieldGraphType>, IList<MetaField>>()
                .Name("MetaFields")
                .Description("Connection to a all meta-fields")
                .ResolveAsync(ctx =>
                {
                    if (!metaFieldServiceProvider.IsEnabled)
                    {
                        throw new ExecutionError("Meta-fields not supported.");
                    }

                    var loader = dataLoaderContextAccessor.Context
                        .GetOrAddBatchLoader<string, IList<MetaField>>("MetaField.LookupByParentIdsAsync", metaFieldLookupService.LookupByParentIdsAsync);
                    return loader.LoadAsync(ctx.Source.Id);
                });

            #endregion Meta-fields
        }
    }
}