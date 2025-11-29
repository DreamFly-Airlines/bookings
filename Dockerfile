FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8082
EXPOSE 8083

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build

ARG GITHUB_USERNAME
RUN --mount=type=secret,id=github_personal_access_token \
    GITHUB_PERSONAL_ACCESS_TOKEN="$(cat /run/secrets/github_personal_access_token)" && \
    dotnet nuget add source "https://nuget.pkg.github.com/DreamFly-Airlines/index.json" \
    --name "github" \
    --username $GITHUB_USERNAME \
    --password $GITHUB_PERSONAL_ACCESS_TOKEN \
    --store-password-in-clear-text

ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Bookings.Api/Bookings.Api.csproj", "src/Bookings.Api/"]
COPY ["src/Bookings.Application/Bookings.Application.csproj", "src/Bookings.Application/"]
COPY ["src/Bookings.Domain/Bookings.Domain.csproj", "src/Bookings.Domain/"]
COPY ["src/Bookings.Infrastructure/Bookings.Infrastructure.csproj", "src/Bookings.Infrastructure/"]
RUN dotnet restore "src/Bookings.Api/Bookings.Api.csproj"
COPY . .
WORKDIR "/src/src/Bookings.Api"
RUN dotnet build "./Bookings.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Bookings.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Bookings.Api.dll"]
