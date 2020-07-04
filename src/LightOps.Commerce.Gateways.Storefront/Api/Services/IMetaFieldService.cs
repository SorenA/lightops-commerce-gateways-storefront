using System.Collections.Generic;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Models;

namespace LightOps.Commerce.Gateways.Storefront.Api.Services
{
    public interface IMetaFieldService
    {
        Task<IMetaField> GetByParentAsync(string parentEntityType, string parentEntityId, string name);
        Task<IList<IMetaField>> GetByParentAsync(string parentEntityType, string parentEntityId);
    }
}
