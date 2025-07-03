# Etapa 1: Build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /app

# Copiar y restaurar dependencias
COPY Proyecto.sln .
COPY Proyecto/*.csproj ./Proyecto/
RUN dotnet restore

# Copiar el resto del código
COPY . .
WORKDIR /app/Proyecto
RUN dotnet publish -c Release -o /out

# Etapa 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build /out .

ENTRYPOINT ["dotnet", "Proyecto.dll"]
