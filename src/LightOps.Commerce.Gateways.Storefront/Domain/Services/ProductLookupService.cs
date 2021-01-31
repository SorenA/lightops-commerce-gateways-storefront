﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Services
{
    public class ProductLookupService : IProductLookupService
    {
        private readonly IProductService _productService;

        public ProductLookupService(
            IProductService productService)
        {
            _productService = productService;
        }

        public async Task<IDictionary<string, Product>> LookupByHandleAsync(IEnumerable<string> handles)
        {
            var result = await _productService.GetByHandleAsync(handles.ToList());
            return result.ToDictionary(x => x.Handle);
        }

        public async Task<IDictionary<string, Product>> LookupByIdAsync(IEnumerable<string> ids)
        {
            var result = await _productService.GetByIdAsync(ids.ToList());
            return result.ToDictionary(x => x.Id);
        }
    }
}