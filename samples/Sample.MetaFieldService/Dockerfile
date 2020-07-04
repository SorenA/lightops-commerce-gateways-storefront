#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

# Install gRPC probe
RUN GRPC_HEALTH_PROBE_VERSION=v0.3.1 && \
    curl -s https://github.com/grpc-ecosystem/grpc-health-probe/releases/download/${GRPC_HEALTH_PROBE_VERSION}/grpc_health_probe-linux-amd64 -o /bin/grpc_health_probe && \
    chmod +x /bin/grpc_health_probe

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["samples/Sample.MetaFieldService/Sample.MetaFieldService.csproj", "samples/Sample.MetaFieldService/"]
RUN dotnet restore "samples/Sample.MetaFieldService/Sample.MetaFieldService.csproj"
COPY . .
WORKDIR "/src/samples/Sample.MetaFieldService"
RUN dotnet build "Sample.MetaFieldService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sample.MetaFieldService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.MetaFieldService.dll"]