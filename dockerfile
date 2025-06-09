# Imagen base para runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
# Configurar el puerto de la aplicación
ENV ASPNETCORE_URLS=http://+:80
# Exponer el puerto que usará la aplicación
EXPOSE 80
# Imagen para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
# Copiar archivos de proyecto primero para mejor cache de Docker
COPY .csproj ./
RUN dotnet restore
# Copiar el resto del código
COPY . .
RUN dotnet publish -c Release -o /app/publish
# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "apiFestivos.Presentacion.dll"] 
