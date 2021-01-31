using System.Linq;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Contexts;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types
{
    public sealed class ImageGraphType : ObjectGraphType<Image>
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
                .Resolve(ctx =>
                {
                    var userContext = (StorefrontGraphUserContext)ctx.UserContext;

                    return ctx.Source.AltTexts
                        .FirstOrDefault(x => x.LanguageCode == userContext.LanguageCode)
                        ?.Value;
                });

            Field<FloatGraphType, double>()
                .Name("FocalCenterTop")
                .Description("The focal center of the image from the top ranging 0-1")
                .Resolve(ctx => ctx.Source.FocalCenterTop ?? 0.5);

            Field<FloatGraphType, double>()
                .Name("FocalCenterLeft")
                .Description("The focal center of the image from the left ranging 0-1")
                .Resolve(ctx => ctx.Source.FocalCenterLeft ?? 0.5);
        }
    }
}
