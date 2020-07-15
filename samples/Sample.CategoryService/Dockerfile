#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:3.1-buster-slim AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/core/sdk:3.1-buster AS build
WORKDIR /src
COPY ["samples/Sample.CategoryService/Sample.CategoryService.csproj", "samples/Sample.CategoryService/"]
RUN dotnet restore "samples/Sample.CategoryService/Sample.CategoryService.csproj"
COPY . .
WORKDIR "/src/samples/Sample.CategoryService"
RUN dotnet build "Sample.CategoryService.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Sample.CategoryService.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Sample.CategoryService.dll"]