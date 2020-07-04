using System;
using System.Threading.Tasks;
using Grpc.Net.Client;
using LightOps.Commerce.Gateways.Storefront.Api.Services;
using Microsoft.Extensions.DependencyInjection;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Services
{
    public class GrpcCallerService : IGrpcCallerService
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public GrpcCallerService(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public async Task<TResponse> CallService<TResponse>(string grpcUrl, Func<GrpcChannel, IServiceProvider, Task<TResponse>> grpcFunc)
        {
            // Enable http2 without TLS
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            var grpcChannel = GrpcChannel.ForAddress(grpcUrl);

            try
            {
                using var scope = _serviceScopeFactory.CreateScope();
                return await grpcFunc(grpcChannel, scope.ServiceProvider);
            }
            finally
            {
                // Disable http2 without TLS
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", false);
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", false);
            }
        }
    }
}