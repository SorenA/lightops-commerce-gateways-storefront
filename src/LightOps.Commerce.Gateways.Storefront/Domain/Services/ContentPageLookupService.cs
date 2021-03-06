﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Services
{
    public class ContentPageLookupService : IContentPageLookupService
    {
        private readonly IContentPageService _contentPageService;

        public ContentPageLookupService(
            IContentPageService contentPageService)
        {
            _contentPageService = contentPageService;
        }

        public async Task<IDictionary<string, ContentPage>> LookupByHandleAsync(IEnumerable<string> handles, string languageCode)
        {
            var result = await _contentPageService.GetByHandleAsync(handles.ToList(), languageCode);
            return result.ToDictionary(x => x.Handles
                .FirstOrDefault(ls => ls.LanguageCode == languageCode)?.Value);
        }

        public async Task<IDictionary<string, ContentPage>> LookupByIdAsync(IEnumerable<string> ids)
        {
            var result = await _contentPageService.GetByIdAsync(ids.ToList());
            return result.ToDictionary(x => x.Id);
        }
    }
}