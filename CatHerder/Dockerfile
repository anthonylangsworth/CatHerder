#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/runtime:6.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
ENV DISCORD_APIKEY=${DISCORD_APIKEY}
WORKDIR /src
COPY ["CatHerder/CatHerder.csproj", "CatHerder/"]
RUN dotnet restore "CatHerder/CatHerder.csproj"
COPY . .
WORKDIR "/src/CatHerder"
RUN dotnet build "CatHerder.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CatHerder.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CatHerder.dll"]