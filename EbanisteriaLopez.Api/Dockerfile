# Etapa 1: build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar csproj y restaurar dependencias
COPY *.sln .
COPY EbanisteriaLopez.Api/*.csproj ./EbanisteriaLopez.Api/
COPY EbanisteriaLopez.Shared/*.csproj ./EbanisteriaLopez.Shared/
RUN dotnet restore

# Copiar el resto del código
COPY EbanisteriaLopez.Api/. ./EbanisteriaLopez.Api/
COPY EbanisteriaLopez.Shared/. ./EbanisteriaLopez.Shared/

WORKDIR /src/EbanisteriaLopez.Api

# Compilar en modo Release
RUN dotnet publish -c Release -o /app/publish /p:UseAppHost=false

# Etapa 2: runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS runtime
WORKDIR /app
COPY --from=build /app/publish .

# Puerto que expone la API
EXPOSE 5000

# Variable opcional para el entorno
ENV DOTNET_RUNNING_IN_CONTAINER=true
ENV DOTNET_SYSTEM_GLOBALIZATION_INVARIANT=false

# Ejecutar la app
ENTRYPOINT ["dotnet", "EbanisteriaLopez.Api.dll"]