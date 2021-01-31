using System.Collections.Generic;
using System.Threading.Tasks;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Api.Services
{
    public interface IMetaFieldService
    {
        /// <summary>
        /// Gets a list of meta-field by ids
        /// </summary>
        /// <param name="ids">The ids of the meta-fields</param>
        /// <returns>List of meta-fields, if any</returns>
        Task<IList<MetaField>> GetByIdAsync(IList<string> ids);

        /// <summary>
        /// Gets a map with lists of meta-field by parent ids
        /// </summary>
        /// <param name="parentIds">The parent ids of the meta-fields</param>
        /// <returns>Map of meta-field found, grouped by parent id</returns>
        Task<IDictionary<string, IList<MetaField>>> GetByParentIdsAsync(IList<string> parentIds);

        /// <summary>
        /// Gets a list of paginated content pages by search
        /// </summary>
        /// <param name="parentId">Globally unique identifier of parent to search in meta-fields of</param>
        /// <param name="namespace">Search only in meta-fields with a specific namespace, if any specified</param>
        /// <param name="name">Search only in meta-fields with a specific name, if any specified</param>
        /// <returns>Search result with content pages matching search</returns>
        Task<IList<MetaField>> GetBySearchAsync(string parentId,
                                                string @namespace,
                                                string name);
    }
}
