FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["SatGetsin2.App/SatGetsin2.App.csproj", "SatGetsin2.App/"]
COPY ["SatGetsin2.Core/SatGetsin2.Core.csproj", "SatGetsin2.Core/"]
COPY ["SatGetsin2.Data/SatGetsin2.Data.csproj", "SatGetsin2.Data/"]
COPY ["SatGetsin2.Service/SatGetsin2.Service.csproj", "SatGetsin2.Service/"]
RUN dotnet restore "./SatGetsin2.App/SatGetsin2.App.csproj"
COPY . .
WORKDIR "/src/SatGetsin2.App"
RUN dotnet build "./SatGetsin2.App.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SatGetsin2.App.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SatGetsin2.App.dll"]