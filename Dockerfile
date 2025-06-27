# =====================
# Build stage
# =====================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copia todos los archivos
COPY . .

# Restaura dependencias
RUN dotnet restore

# Publica para producción
RUN dotnet publish -c Release -o /app/publish


# =====================
# Runtime stage
# =====================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# Copia el resultado de la publicación
COPY --from=build /app/publish .

# Exponer el puerto (opcional pero recomendado)
EXPOSE 8080

# Start the app
ENTRYPOINT ["dotnet", "DAEAPF.dll"]
