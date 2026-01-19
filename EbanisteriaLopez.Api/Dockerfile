# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar solo el proyecto API (evita referencias a proyectos inexistentes)
COPY EbanisteriaLopez.Api/*.csproj ./EbanisteriaLopez.Api/

# Restaurar dependencias usando el csproj concreto
RUN dotnet restore EbanisteriaLopez.Api/EbanisteriaLopez.Api.csproj

# Copiar el código fuente del proyecto
COPY EbanisteriaLopez.Api/. ./EbanisteriaLopez.Api/

WORKDIR /src/EbanisteriaLopez.Api

# Publicar la aplicación en modo Release
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app

# Copiar los archivos publicados desde la etapa de build
COPY --from=build /app/publish .

# Exponer el puerto estándar en contenedores y configurar la URL de escucha
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Comando de inicio
ENTRYPOINT ["dotnet", "EbanisteriaLopez.Api.dll"]