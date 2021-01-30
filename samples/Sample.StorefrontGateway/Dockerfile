#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0-buster-slim AS base
WORKDIR /app
EXPOSE 80

# Install packages
RUN apt-get update \
    && apt-get install --no-install-recommends -y \
        curl \
    && apt-get clean \
    && rm -rf /var/lib/apt/lists/*

# Install gRPC probe
RUN GRPC_HEALTH_PROBE_VERSION=v0.3.1 && \
    curl -s https://github.com/grpc-ecosystem/grpc-health-probe/releases/download/${GRPC_HEALTH_PROBE_VERSION}/grpc_health_probe-linux-amd64 -o /bin/grpc_health_probe && \
    chmod +x /bin/grpc_health_probe

FROM mcr.microsoft.com/dotnet/sdk:5.0.102-ca-patch-buster-slim AS build
WORKDIR /src
COPY ["samples/Sample.StorefrontGateway/Sample.StorefrontGateway.csproj", "samples/Sample.StorefrontGateway/"]
RUN dotnet restore "samples/Sample.StorefrontGateway/Sample.StorefrontGateway.csproj"
COPY . .
WORKDIR "/src/samples/Sample.StorefrontGateway"
RUN dotnet build "Sample.StorefrontGateway.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sample.StorefrontGateway.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.StorefrontGateway.dll"]