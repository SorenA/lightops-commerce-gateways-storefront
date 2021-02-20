using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Services
{
    public class NavigationLookupService : INavigationLookupService
    {
        private readonly INavigationService _navigationService;

        public NavigationLookupService(
            INavigationService navigationService)
        {
            _navigationService = navigationService;
        }

        public async Task<IDictionary<string, Navigation>> LookupByHandleAsync(IEnumerable<string> handles, string languageCode)
        {
            var result = await _navigationService.GetByHandleAsync(handles.ToList(), languageCode);
            return result.ToDictionary(x => x.Handles
                .FirstOrDefault(ls => ls.LanguageCode == languageCode)?.Value);
        }

        public async Task<IDictionary<string, Navigation>> LookupByIdAsync(IEnumerable<string> ids)
        {
            var result = await _navigationService.GetByIdAsync(ids.ToList());
            return result.ToDictionary(x => x.Id);
        }
    }
}