FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["FitSync.API/FitSync.API.csproj", "FitSync.API/"]
COPY ["FitSync.Application/FitSync.Application.csproj", "FitSync.Application/"]
COPY ["FitSync.Domain/FitSync.Domain.csproj", "FitSync.Domain/"]
COPY ["FitSync.Infrastructure/FitSync.Infrastructure.csproj", "FitSync.Infrastructure/"]
RUN dotnet restore "FitSync.API/FitSync.API.csproj"
COPY . .
WORKDIR "/src/FitSync.API"
RUN dotnet build "FitSync.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "FitSync.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FitSync.API.dll"]
