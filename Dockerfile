# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8081


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Blogtify/Blogtify/Blogtify.csproj", "Blogtify/Blogtify/"]
COPY ["Blogtify/Blogtify.Client/Blogtify.Client.csproj", "Blogtify/Blogtify.Client/"]
RUN dotnet restore "./Blogtify/Blogtify/Blogtify.csproj"

# Install wasm-tools
RUN dotnet workload install wasm-tools-net8

COPY . .
WORKDIR "/src/Blogtify/Blogtify"
RUN dotnet build "./Blogtify.csproj" -c $BUILD_CONFIGURATION -o /app/build

# Publish server
RUN dotnet publish "Blogtify.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# Publish client ReleaseCompat
WORKDIR "/src/Blogtify/Blogtify.Client"
RUN dotnet publish "Blogtify.Client.csproj" -c ReleaseCompat -o /app/publishCompat --no-restore

# Copy compat framework into server's publish
RUN mkdir -p /app/publish/wwwroot/_frameworkCompat && \
    cp -r /app/publishCompat/wwwroot/_framework/* /app/publish/wwwroot/_frameworkCompat/

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=build /app/publish .

ENV ASPNETCORE_URLS=http://0.0.0.0:${PORT}

ENTRYPOINT ["dotnet", "Blogtify.dll"]