# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build-env
WORKDIR /app

COPY . ./

RUN dotnet restore Api/OrderManagement.Api/OrderManagement.Api.csproj

RUN dotnet publish Api/OrderManagement.Api/OrderManagement.Api.csproj -c Release -o out --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app
COPY --from=build-env /app/out .

RUN mkdir -p /var/lib/render/app-data

ENV ASPNETCORE_URLS=http://+:10000
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 10000

ENTRYPOINT ["dotnet", "OrderManagement.Api.dll"]
