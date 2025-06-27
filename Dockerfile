# =====================
# Build stage
# =====================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# Copiar todo el código
COPY . .

# Restaurar dependencias de toda la solución
RUN dotnet restore DAEAPF.sln

# Publicar SOLO la capa API (entry point)
RUN dotnet publish DAEAPF/DAEAPF.csproj -c Release -o /app/publish


# =====================
# Runtime stage
# =====================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

COPY --from=build /app/publish .

EXPOSE 8080

ENTRYPOINT ["dotnet", "DAEAPF.dll"]
