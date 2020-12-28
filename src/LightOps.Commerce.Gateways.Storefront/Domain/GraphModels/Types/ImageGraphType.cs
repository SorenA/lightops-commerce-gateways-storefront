using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class ImageGraphType : ObjectGraphType<IImage>
    {
        public ImageGraphType(IImageCdnProvider imageCdnProvider)
        {
            Name = "Image";

            Field<StringGraphType, string>()
                .Name("Id")
                .Description("Globally unique identifier, eg: gid://Image/1000")
                .Resolve(ctx => ctx.Source.Id);

            Field<StringGraphType, string>()
                .Name("Url")
                .Description("The url where the image may be accessed")
                .Resolve(ctx =>
                {
                    // Check if image CDN is enabled
                    if (imageCdnProvider.IsEnabled && ctx.Source.Url.StartsWith("/"))
                    {
                        // CDN is enabled, url is relative
                        return $"{imageCdnProvider.CdnHost}{ctx.Source.Url}";
                    }

                    return ctx.Source.Url;
                });

            Field<StringGraphType, string>()
                .Name("AltText")
                .Description("The alt text of the image")
                .Resolve(ctx => ctx.Source.AltText);

            Field<FloatGraphType, double>()
                .Name("FocalCenterTop")
                .Description("The focal center of the image from the top ranging 0-1")
                .Resolve(ctx => ctx.Source.FocalCenterTop);

            Field<FloatGraphType, double>()
                .Name("FocalCenterLeft")
                .Description("The focal center of the image from the left ranging 0-1")
                .Resolve(ctx => ctx.Source.FocalCenterLeft);
        }
    }
}
