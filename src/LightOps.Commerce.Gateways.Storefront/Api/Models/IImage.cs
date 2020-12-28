namespace LightOps.Commerce.Gateways.Storefront.Api.Models
{
    public interface IImage
    {
        /// <summary>
        /// Globally unique identifier, eg: gid://Image/1000
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// The url where the image may be accessed
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// The alt text of the image
        /// </summary>
        string AltText { get; set; }

        /// <summary>
        /// The focal center of the image from the top ranging 0-1
        /// </summary>
        double FocalCenterTop { get; set; }

        /// <summary>
        /// The focal center of the image from the left ranging 0-1
        /// </summary>
        double FocalCenterLeft { get; set; }
    }
}