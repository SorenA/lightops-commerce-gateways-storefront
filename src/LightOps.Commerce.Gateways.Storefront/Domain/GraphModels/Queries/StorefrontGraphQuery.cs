﻿using System.Collections.Generic;
using System.Threading.Tasks;
using GraphQL;
using GraphQL.Types;
using LightOps.Commerce.Gateways.Storefront.Api.Models;
using LightOps.Commerce.Gateways.Storefront.Api.Providers;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Types;
using Microsoft.Extensions.DependencyInjection;

namespace LightOps.Commerce.Gateways.Storefront.Domain.GraphModels.Queries
{
    public class StorefrontGraphQuery : ObjectGraphType
    {
        private readonly IContentPageEndpointProvider _contentPageEndpointProvider;
        private readonly INavigationEndpointProvider _navigationEndpointProvider;
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public StorefrontGraphQuery(
            IContentPageEndpointProvider contentPageEndpointProvider,
            INavigationEndpointProvider navigationEndpointProvider,
            IServiceScopeFactory serviceScopeFactory)
        {
            _contentPageEndpointProvider = contentPageEndpointProvider;
            _navigationEndpointProvider = navigationEndpointProvider;
            _serviceScopeFactory = serviceScopeFactory;

            Name = "Query";

            AddContentPageFields();
            AddNavigationFields();
        }

        #region Content Pages
        private void AddContentPageFields()
        {
            Field<ContentPageGraphType>("contentPage",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "id" },
                    new QueryArgument<StringGraphType> { Name = "handle" }
                ),
                resolve: ResolveContentPage);

            Field<ListGraphType<ContentPageGraphType>>("contentPages",
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

            using var scope = _serviceScopeFactory.CreateScope();
            var contentPageService = scope.ServiceProvider.GetService<IContentPageService>();

            if (context.HasArgument("id"))
            {
                return contentPageService.GetByIdAsync(context.GetArgument<string>("id"));
            }

            if (context.HasArgument("handle"))
            {
                return contentPageService.GetByHandleAsync(context.GetArgument<string>("handle"));
            }

            return null;
        }

        private Task<IList<IContentPage>> ResolveContentPages(IResolveFieldContext<object> context)
        {
            if (!_contentPageEndpointProvider.IsEnabled)
            {
                throw new ExecutionError("Content pages not supported.");
            }

            using var scope = _serviceScopeFactory.CreateScope();
            var contentPageService = scope.ServiceProvider.GetService<IContentPageService>();

            if (context.HasArgument("parentId"))
            {
                return contentPageService.GetByParentIdAsync(context.GetArgument<string>("parentId"));
            }

            if (context.HasArgument("searchTerm"))
            {
                return contentPageService.GetBySearchAsync(context.GetArgument<string>("searchTerm"));
            }

            // Fallback to root
            return contentPageService.GetByRootAsync();
        }
        #endregion Content Pages

        #region Navigations
        private void AddNavigationFields()
        {
            Field<NavigationGraphType>("navigation",
                arguments: new QueryArguments(
                    new QueryArgument<StringGraphType> { Name = "id" },
                    new QueryArgument<StringGraphType> { Name = "handle" }
                ),
                resolve: ResolveNavigation);

            Field<ListGraphType<NavigationGraphType>>("navigations",
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

            using var scope = _serviceScopeFactory.CreateScope();
            var navigationService = scope.ServiceProvider.GetService<INavigationService>();

            if (context.HasArgument("id"))
            {
                return navigationService.GetByIdAsync(context.GetArgument<string>("id"));
            }

            if (context.HasArgument("handle"))
            {
                return navigationService.GetByHandleAsync(context.GetArgument<string>("handle"));
            }

            return null;
        }

        private Task<IList<INavigation>> ResolveNavigations(IResolveFieldContext<object> context)
        {
            if (!_navigationEndpointProvider.IsEnabled)
            {
                throw new ExecutionError("Navigations not supported.");
            }

            using var scope = _serviceScopeFactory.CreateScope();
            var navigationService = scope.ServiceProvider.GetService<INavigationService>();

            if (context.HasArgument("parentId"))
            {
                return navigationService.GetByParentIdAsync(context.GetArgument<string>("parentId"));
            }

            // Fallback to root
            return navigationService.GetByRootAsync();
        }
        #endregion Navigations
    }
}
