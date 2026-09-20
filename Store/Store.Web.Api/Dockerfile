FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Directory.Build.props", "./"]
COPY ["Store.Domain/Store.Domain.csproj", "Store.Domain/"]
COPY ["Store.Application/Store.Application.csproj", "Store.Application/"]
COPY ["Store.Infrastructure/Store.Infrastructure.csproj", "Store.Infrastructure/"]
COPY ["Store.Persistence/Store.Persistence.csproj", "Store.Persistence/"]
COPY ["Store.Web.Api/Store.Web.Api.csproj", "Store.Web.Api/"]
RUN dotnet restore "Store.Web.Api/Store.Web.Api.csproj"
COPY . .
RUN dotnet publish "Store.Web.Api/Store.Web.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Store.Web.Api.dll"]
