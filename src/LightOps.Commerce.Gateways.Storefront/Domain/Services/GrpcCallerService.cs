using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Net.Client;
using LightOps.Commerce.Gateways.Storefront.Api.Services;

namespace LightOps.Commerce.Gateways.Storefront.Domain.Services
{
    public class GrpcCallerService : IGrpcCallerService
    {
        public async Task<TResponse> CallService<TResponse>(string grpcUrl, Func<GrpcChannel, Task<TResponse>> grpcFunc)
        {
            // Enable http2 without TLS
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2Support", true);

            var grpcChannel = GrpcChannel.ForAddress(grpcUrl, new GrpcChannelOptions
            {
                Credentials = ChannelCredentials.Insecure
            });

            return await grpcFunc(grpcChannel);
        }
    }
}