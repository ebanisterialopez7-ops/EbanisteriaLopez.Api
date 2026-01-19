# Etapa 1: imagen base runtime (.NET 8)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Etapa 2: imagen SDK para compilar (.NET 8)
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar sólo el proyecto API (evita referencias a proyectos Shared inexistentes)
COPY EbanisteriaLopez.Api/*.csproj ./EbanisteriaLopez.Api/
WORKDIR /src/EbanisteriaLopez.Api
RUN dotnet restore "EbanisteriaLopez.Api.csproj"

# Copiar el resto del código y publicar
COPY EbanisteriaLopez.Api/. .
RUN dotnet publish "EbanisteriaLopez.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

# Etapa final: runtime
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "EbanisteriaLopez.Api.dll"]