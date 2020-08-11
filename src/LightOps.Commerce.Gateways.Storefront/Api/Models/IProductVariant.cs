using System.Collections.Generic;
using NodaMoney;

namespace LightOps.Commerce.Gateways.Storefront.Api.Models
{
    public interface IProductVariant
    {
        /// <summary>
        /// Globally unique identifier, eg: gid://ProductVariant/1000
        /// </summary>
        string Id { get; set; }

        /// <summary>
        /// Globally unique identifier of the parent product
        /// </summary>
        string ProductId { get; set; }

        /// <summary>
        /// The title of the product variant
        /// </summary>
        string Title { get; set; }

        /// <summary>
        /// The stock keeping unit of the product variant
        /// </summary>
        string Sku { get; set; }

        /// <summary>
        /// The unit price of the product variant
        /// </summary>
        Money UnitPrice { get; set; }


        /// <summary>
        /// The images of the product variant
        /// </summary>
        IList<IImage> Images { get; set; }
    }
}