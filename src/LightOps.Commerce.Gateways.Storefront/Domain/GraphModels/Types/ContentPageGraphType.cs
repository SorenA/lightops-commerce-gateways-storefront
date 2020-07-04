using GraphQL;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class ContentPageGraphType : ObjectGraphType<IContentPage>
    {
        public ContentPageGraphType(
            IMetaFieldEndpointProvider metaFieldEndpointProvider,
            IMetaFieldService metaFieldService
            )
        {
            Name = "ContentPage";

            Field(m => m.Id);
            Field(m => m.Handle);
            Field(m => m.Url);

            Field(m => m.ParentId, true);

            Field(m => m.Title);
            Field(m => m.Type);
            Field(m => m.Description);

            Field(m => m.SeoTitle);
            Field(m => m.SeoDescription);

            Field(m => m.PrimaryImage);

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

                    return metaFieldService.GetByParentAsync("content_page", context.Source.Id,
                        context.GetArgument<string>("name"));
                });
            Field<ListGraphType<MetaFieldGraphType>>("MetaFields",
                resolve: context =>
                {
                    if (!metaFieldEndpointProvider.IsEnabled)
                    {
                        throw new ExecutionError("Meta-fields not supported.");
                    }

                    return metaFieldService.GetByParentAsync("content_page", context.Source.Id);
                });
        }
    }
}
