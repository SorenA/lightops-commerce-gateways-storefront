using GraphQL;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class ProductGraphType : ObjectGraphType<IProduct>
    {
        public ProductGraphType(
            IMetaFieldEndpointProvider metaFieldEndpointProvider,
            IMetaFieldService metaFieldService,
            ICategoryService categoryService
            )
        {
            Name = "Product";

            Field(m => m.Id);
            Field(m => m.Handle);
            Field(m => m.Url);

            Field(m => m.Title);
            Field(m => m.Type);
            Field(m => m.Description);

            Field(m => m.SeoTitle);
            Field(m => m.SeoDescription);

            Field(m => m.PrimaryCategoryId);
            Field(m => m.CategoryIds);

            Field<ListGraphType<ProductVariantGraphType>>("Variants", resolve: context => context.Source.Variants);

            Field(m => m.PrimaryImage);
            Field(m => m.Images);

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

                    return metaFieldService.GetByParentAsync("product", context.Source.Id,
                        context.GetArgument<string>("name"));
                });
            Field<ListGraphType<MetaFieldGraphType>>("MetaFields",
                resolve: context =>
                {
                    if (!metaFieldEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Meta-fields not supported.");
                    }

                    return metaFieldService.GetByParentAsync("product", context.Source.Id);
                });

            // Category hierarchy
            Field<CategoryGraphType>("PrimaryCategory",
                resolve: context => categoryService.GetByIdAsync(context.Source.PrimaryCategoryId));
        }
    }
}
