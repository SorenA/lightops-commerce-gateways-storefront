using System;

namespace LightOps.Commerce.Gateways.Storefront.Api.Models
{
    public interface IContentPage
    {
        /// <summary>
        /// Globally unique identifier, eg: gid://ContentPage/1000
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Globally unique identifier of parent, 'gid://' if none
        /// </summary>
        string ParentId { get; set; }

        /// <summary>
        /// A human-friendly unique string for the content page
        /// </summary>
        string Handle { get; set; }

        /// <summary>
        /// The title of the content page
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// The url of the content page
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// The type of the content page
        /// </summary>
        string Type { get; set; }

        /// <summary>
        /// The summary of the content page
        /// </summary>
        string Summary { get; set; }

        /// <summary>
        /// The timestamp of content page creation
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// The timestamp of the latest content page update
        /// </summary>
        DateTime UpdatedAt { get; set; }

        /// <summary>
        /// The primary image of the content page
        /// </summary>
        IImage PrimaryImage { get; set; }
    }
}
