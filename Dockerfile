FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["ChampionsChromo.sln", "./"]
COPY ["src/ChampionsChromo.Api/ChampionsChromo.Api.csproj", "src/ChampionsChromo.Api/"]
COPY ["src/ChampionsChromo.Application/ChampionsChromo.Application.csproj", "src/ChampionsChromo.Application/"]
COPY ["src/ChampionsChromo.Core/ChampionsChromo.Core.csproj", "src/ChampionsChromo.Core/"]
COPY ["src/ChampionsChromo.Infrastructure/ChampionsChromo.Infrastructure.csproj", "src/ChampionsChromo.Infrastructure/"]

RUN dotnet restore "ChampionsChromo.sln"
COPY . .
WORKDIR "/src/src/ChampionsChromo.Api"
RUN dotnet build "ChampionsChromo.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "ChampionsChromo.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "ChampionsChromo.Api.dll"]