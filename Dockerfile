# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar todos los archivos de la solución
COPY Proyecto.sln ./
COPY Proyecto/ Proyecto/
COPY Application/ Application/
COPY Domain/ Domain/
COPY Infraestructure/ Infraestructure/

# Restaurar dependencias
RUN dotnet restore Proyecto.sln

# Publicar el proyecto principal
WORKDIR /app/Proyecto
RUN dotnet publish -c Release -o /out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

ENTRYPOINT ["dotnet", "Proyecto.dll"]
