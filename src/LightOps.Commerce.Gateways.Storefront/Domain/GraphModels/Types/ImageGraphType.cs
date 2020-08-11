using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class ImageGraphType : ObjectGraphType<IImage>
    {
        public ImageGraphType()
        {
            Name = "Image";

            Field<StringGraphType, string>()
                .Name("Id")
                .Description("Globally unique identifier, eg: gid://Image/1000")
                .Resolve(ctx => ctx.Source.Id);

            Field<StringGraphType, string>()
                .Name("Url")
                .Description("The url where the image may be accessed")
                .Resolve(ctx => ctx.Source.Url);

            Field<StringGraphType, string>()
                .Name("AltText")
                .Description("The alt text of the image")
                .Resolve(ctx => ctx.Source.AltText);
        }
    }
}
