#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0-alpine AS base
WORKDIR /app
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["HodlCoin/Server/HodlCoin.Server.csproj", "HodlCoin/Server/"]
COPY ["HodlCoin/Client/HodlCoin.Client.csproj", "HodlCoin/Client/"]
COPY ["HodlCoin/Shared/HodlCoin.Shared.csproj", "HodlCoin/Shared/"]
RUN dotnet restore "HodlCoin/Server/HodlCoin.Server.csproj"
COPY . .
WORKDIR "/src/HodlCoin/Server"
RUN dotnet build "HodlCoin.Server.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "HodlCoin.Server.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "HodlCoin.Server.dll"]