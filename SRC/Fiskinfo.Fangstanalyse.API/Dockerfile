#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2-stretch-slim AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/core/sdk:2.2-stretch AS build
WORKDIR /src
COPY ["SRC/Fiskinfo.Fangstanalyse.API/Fiskinfo.Fangstanalyse.API.csproj", "SRC/Fiskinfo.Fangstanalyse.API/"]
COPY ["SRC/Fiskinfo.Fangstanalyse.Infrastructure/Fiskinfo.Fangstanalyse.Infrastructure.csproj", "SRC/Fiskinfo.Fangstanalyse.Infrastructure/"]
RUN dotnet restore "SRC/Fiskinfo.Fangstanalyse.API/Fiskinfo.Fangstanalyse.API.csproj"
COPY . .
WORKDIR "/src/SRC/Fiskinfo.Fangstanalyse.API"
RUN dotnet build "Fiskinfo.Fangstanalyse.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Fiskinfo.Fangstanalyse.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Fiskinfo.Fangstanalyse.API.dll"]