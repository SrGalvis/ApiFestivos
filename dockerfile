# Imagen base para runtime
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:80
EXPOSE 80

# Imagen para build
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar archivos de proyecto primero para mejor cache
COPY apiFestivos.Presentacion/apiFestivos.Presentacion.csproj ./apiFestivos.Presentacion/
RUN dotnet restore ./apiFestivos.Presentacion/apiFestivos.Presentacion.csproj

# Copiar el resto del código
COPY . .
WORKDIR /src/apiFestivos.Presentacion
RUN dotnet publish -c Release -o /app/publish

# Imagen final
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "apiFestivos.Presentacion.dll"]
