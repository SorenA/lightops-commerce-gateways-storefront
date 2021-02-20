using System.Collections.Generic;
using System.Threading.Tasks;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Api.Services
{
    public interface INavigationService
    {
        /// <summary>
        /// Gets a list of navigations by handle
        /// </summary>
        /// <param name="handles">The handles of the navigations</param>
        /// <param name="languageCode">The language code to match handles in<br/>ISO 639 2-letter language code matched with ISO 3166 2-letter country code, eg. en-US, da-DK</param>
        /// <returns>List of navigations, if any</returns>
        Task<IList<Navigation>> GetByHandleAsync(IList<string> handles,
                                                 string languageCode);

        /// <summary>
        /// Gets a list of navigations by ids
        /// </summary>
        /// <param name="ids">The ids of the navigations</param>
        /// <returns>List of navigations, if any</returns>
        Task<IList<Navigation>> GetByIdAsync(IList<string> ids);
    }
}