#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 18137

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build

WORKDIR /src
COPY ["Lyrida.Api/Lyrida.Api.csproj", "Lyrida.Api/"]
COPY ["Lyrida.Application/Lyrida.Application.csproj", "Lyrida.Application/"]
COPY ["Lyrida.DataAccess/Lyrida.DataAccess.csproj", "Lyrida.DataAccess/"]
COPY ["Lyrida.Domain/Lyrida.Domain.csproj", "Lyrida.Domain/"]
COPY ["Lyrida.Infrastructure/Lyrida.Infrastructure.csproj", "Lyrida.Infrastructure/"]
RUN dotnet restore "Lyrida.Api/Lyrida.Api.csproj"
COPY . .
WORKDIR "/src/Lyrida.Api"
RUN dotnet build "Lyrida.Api.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Lyrida.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lyrida.Api.dll"]