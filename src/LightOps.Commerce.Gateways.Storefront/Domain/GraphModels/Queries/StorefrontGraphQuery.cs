using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types;
using LightOps.Mapping.Api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Queries
{
    public class StorefrontGraphQuery : ObjectGraphType
    {
        private readonly IContentPageEndpointProvider _contentPageEndpointProvider;
        private readonly INavigationEndpointProvider _navigationEndpointProvider;
        private readonly IContentPageService _contentPageService;
        private readonly INavigationService _navigationService;

        public StorefrontGraphQuery(
            IContentPageEndpointProvider contentPageEndpointProvider,
            INavigationEndpointProvider navigationEndpointProvider,
            IContentPageService contentPageService,
            INavigationService navigationService)
        {
            _contentPageEndpointProvider = contentPageEndpointProvider;
            _navigationEndpointProvider = navigationEndpointProvider;
            _contentPageService = contentPageService;
            _navigationService = navigationService;

            Name = "Query";

            AddContentPageFields();
            AddNavigationFields();
        }

        #region Content Pages
        private void AddContentPageFields()
        {
            Field<ContentPageGraphType>("ContentPage",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "id" },
                    new QueryArgument<StringGraphType> { Name = "handle" }
                ),
                resolve: ResolveContentPage);

            Field<ListGraphType<ContentPageGraphType>>("ContentPages",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "parentId" },
                    new QueryArgument<StringGraphType> { Name = "searchTerm" }
                ),
                resolve: ResolveContentPages);
        }

        private Task<IContentPage> ResolveContentPage(IResolveFieldContext<object> context)
        {
            if (!_contentPageEndpointProvider.IsEnabled)
            {
                throw new ExecutionError("Content pages not supported.");
            }

            if (context.HasArgument("id"))
            {
                return _contentPageService.GetByIdAsync(context.GetArgument<string>("id"));
            }

            if (context.HasArgument("handle"))
            {
                return _contentPageService.GetByHandleAsync(context.GetArgument<string>("handle"));
            }

            return null;
        }

        private Task<IList<IContentPage>> ResolveContentPages(IResolveFieldContext<object> context)
        {
            if (!_contentPageEndpointProvider.IsEnabled)
            {
                throw new ExecutionError("Content pages not supported.");
            }

            if (context.HasArgument("parentId"))
            {
                return _contentPageService.GetByParentIdAsync(context.GetArgument<string>("parentId"));
            }

            if (context.HasArgument("searchTerm"))
            {
                return _contentPageService.GetBySearchAsync(context.GetArgument<string>("searchTerm"));
            }

            // Fallback to root
            return _contentPageService.GetByRootAsync();
        }
        #endregion Content Pages

        #region Navigations
        private void AddNavigationFields()
        {
            Field<NavigationGraphType>("Navigation",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "id" },
                    new QueryArgument<StringGraphType> { Name = "handle" }
                ),
                resolve: ResolveNavigation);

            Field<ListGraphType<NavigationGraphType>>("Navigations",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "parentId" }
                ),
                resolve: ResolveNavigations);
        }

        private Task<INavigation> ResolveNavigation(IResolveFieldContext<object> context)
        {
            if (!_navigationEndpointProvider.IsEnabled)
            {
                throw new ExecutionError("Navigations not supported.");
            }
            
            if (context.HasArgument("id"))
            {
                return _navigationService.GetByIdAsync(context.GetArgument<string>("id"));
            }

            if (context.HasArgument("handle"))
            {
                return _navigationService.GetByHandleAsync(context.GetArgument<string>("handle"));
            }

            return null;
        }

        private Task<IList<INavigation>> ResolveNavigations(IResolveFieldContext<object> context)
        {
            if (!_navigationEndpointProvider.IsEnabled)
            {
                throw new ExecutionError("Navigations not supported.");
            }

            if (context.HasArgument("parentId"))
            {
                return _navigationService.GetByParentIdAsync(context.GetArgument<string>("parentId"));
            }

            // Fallback to root
            return _navigationService.GetByRootAsync();
        }
        #endregion Navigations
    }
}
