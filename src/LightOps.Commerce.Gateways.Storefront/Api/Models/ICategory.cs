using System;

namespace LightOps.Commerce.Gateways.Storefront.Api.Models
{
    public interface ICategory
    {
        /// <summary>
        /// Globally unique identifier, eg: gid://Category/1000
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Globally unique identifier of parent, 'gid://' if none
        /// </summary>
        string ParentId { get; set; }

        /// <summary>
        /// A human-friendly unique string for the category
        /// </summary>
        string Handle { get; set; }

        /// <summary>
        /// The title of the category
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// The url of the category
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// The type of the category
        /// </summary>
        string Type { get; set; }

        /// <summary>
        /// The description of the category
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// The timestamp of category creation
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// The timestamp of the latest category update
        /// </summary>
        DateTime UpdatedAt { get; set; }

        /// <summary>
        /// The primary image of the category
        /// </summary>
        IImage PrimaryImage { get; set; }
    }
}