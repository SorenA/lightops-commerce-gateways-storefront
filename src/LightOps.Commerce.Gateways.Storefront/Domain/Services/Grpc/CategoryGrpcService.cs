using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.QueryResults;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Proto.Services.Category;
using LightOps.Commerce.Proto.Types;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Services.Grpc
{
    public class CategoryGrpcService : ICategoryService
    {
        private readonly ICategoryServiceProvider _categoryServiceProvider;
        private readonly IGrpcCallerService _grpcCallerService;

        public CategoryGrpcService(
            ICategoryServiceProvider categoryServiceProvider,
            IGrpcCallerService grpcCallerService)
        {
            _categoryServiceProvider = categoryServiceProvider;
            _grpcCallerService = grpcCallerService;
        }

        public async Task<IList<Category>> GetByHandleAsync(IList<string> handles,
                                                            string languageCode)
        {
            return await _grpcCallerService.CallService(_categoryServiceProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new CategoryService.CategoryServiceClient(grpcChannel);
                var response = await client.GetByHandlesAsync(new GetByHandlesRequest
                {
                    Handles = {handles}
                });

                // Filter out matches in other languages
                return response.Categories
                    .Where(x => x.Handles
                        .Any(ls => ls.LanguageCode == languageCode && handles.Contains(ls.Value)))
                    .ToList();
            });
        }

        public async Task<IList<Category>> GetByIdAsync(IList<string> ids)
        {
            return await _grpcCallerService.CallService(_categoryServiceProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new CategoryService.CategoryServiceClient(grpcChannel);
                var response = await client.GetByIdsAsync(new GetByIdsRequest
                {
                    Ids = {ids}
                });

                return response.Categories;
            });
        }

        public async Task<SearchResult<Category>> GetBySearchAsync(string searchTerm,
                                                                    string languageCode,
                                                                    string parentId,
                                                                    string pageCursor,
                                                                    int pageSize,
                                                                    CategorySortKey sortKey,
                                                                    bool reverse)
        {
            return await _grpcCallerService.CallService(_categoryServiceProvider.GrpcEndpoint, async (grpcChannel) =>
            {
                var client = new CategoryService.CategoryServiceClient(grpcChannel);
                var response = await client.GetBySearchAsync(new GetBySearchRequest
                {
                    SearchTerm = searchTerm,
                    LanguageCode = languageCode,
                    ParentId = parentId,
                    PageCursor = pageCursor,
                    PageSize = pageSize,
                    SortKey = sortKey,
                    Reverse = reverse,
                });

                var results = response
                    .Results
                    .Select(x => new CursorNodeResult<Category>
                    {
                        Cursor = x.Cursor,
                        Node = x.Node
                    })
                    .ToList();

                return new SearchResult<Category>
                {
                    HasNextPage = response.HasNextPage,
                    HasPreviousPage = response.HasPreviousPage,
                    StartCursor = response.StartCursor,
                    EndCursor = response.EndCursor,
                    TotalResults = response.TotalResults,
                    Results = results,
                };
            });
        }
    }
}