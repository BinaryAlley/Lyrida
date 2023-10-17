#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:7.0 AS base
WORKDIR /app
EXPOSE 18138

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /src
COPY ["Lyrida.UI/Lyrida.UI.csproj", "Lyrida.UI/"]
COPY ["Lyrida.Infrastructure/Lyrida.Infrastructure.csproj", "Lyrida.Infrastructure/"]
RUN dotnet restore "Lyrida.UI/Lyrida.UI.csproj"
COPY . .
WORKDIR "/src/Lyrida.UI"
RUN dotnet build "Lyrida.UI.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "Lyrida.UI.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Lyrida.UI.dll"]