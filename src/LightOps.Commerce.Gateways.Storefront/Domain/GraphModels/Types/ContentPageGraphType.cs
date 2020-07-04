using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class ContentPageGraphType : ObjectGraphType<IContentPage>
    {
        public ContentPageGraphType()
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
        }
    }
}
