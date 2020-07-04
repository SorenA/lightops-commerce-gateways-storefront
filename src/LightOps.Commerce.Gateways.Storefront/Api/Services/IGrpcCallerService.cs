using System;
using System.Threading.Tasks;
using Grpc.Net.Client;

namespace LightOps.Commerce.Gateways.Storefront.Api.Services
{
    public interface IGrpcCallerService
    {
        Task<TResponse> CallService<TResponse>(string grpcUrl, Func<GrpcChannel, Task<TResponse>> grpcFunc);
    }
}