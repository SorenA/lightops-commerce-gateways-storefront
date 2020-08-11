namespace LightOps.Commerce.Gateways.Storefront.Api.Models
{
    public interface INavigationLink
    {
        /// <summary>
        /// The title of the link
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// The url of the link, if any
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// The target of the link, if any
        /// </summary>
        string Target { get; set; }
    }
}