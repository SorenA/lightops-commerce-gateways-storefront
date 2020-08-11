using System;
using System.Collections.Generic;

namespace LightOps.Commerce.Gateways.Storefront.Api.Models
{
    public interface IProduct
    {
        /// <summary>
        /// Globally unique identifier, eg: gid://Product/1000
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Globally unique identifier of parent, 'gid://' if none
        /// </summary>
        string ParentId { get; set; }

        /// <summary>
        /// A human-friendly unique string for the product
        /// </summary>
        string Handle { get; set; }

        /// <summary>
        /// The title of the product
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// The url of the product
        /// </summary>
        string Url { get; set; }

        /// <summary>
        /// The type of the product
        /// </summary>
        string Type { get; set; }

        /// <summary>
        /// The description of the product
        /// </summary>
        string Description { get; set; }

        /// <summary>
        /// The timestamp of product creation
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// The timestamp of the latest product update
        /// </summary>
        DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Globally unique identifier of the primary category the product belong to
        /// </summary>
        string PrimaryCategoryId { get; set; }

        /// <summary>
        /// Globally unique identifiers of categories the product belong to
        /// </summary>
        IList<string> CategoryIds { get; set; }

        /// <summary>
        /// The variants of the product
        /// </summary>
        IList<IProductVariant> Variants { get; set; }

        /// <summary>
        /// The images of the product
        /// </summary>
        IList<IImage> Images { get; set; }
    }
}