using System;

namespace LightOps.Commerce.Gateways.Storefront.Api.Models
{
    public interface IMetaField
    {
        /// <summary>
        /// Globally unique identifier, eg: gid://MetaField/1000
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Globally unique identifier of parent, 'gid://' if none
        /// </summary>
        string ParentId { get; set; }

        /// <summary>
        /// The namespace of the meta-field
        /// </summary>
        string Namespace { get; set; }

        /// <summary>
        /// The name of the meta-field
        /// </summary>
        string Name { get; set; }
        
        /// <summary>
        /// The type of the meta-field
        /// </summary>
        string Type { get; set; }

        /// <summary>
        /// The value of the meta-field
        /// </summary>
        string Value { get; set; }

        /// <summary>
        /// The timestamp of meta-field creation
        /// </summary>
        DateTime CreatedAt { get; set; }

        /// <summary>
        /// The timestamp of the latest meta-field update
        /// </summary>
        DateTime UpdatedAt { get; set; }
    }
}