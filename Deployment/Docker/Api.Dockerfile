# our base image will be the .net core 3.1 sdk
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers, like package.json in node projects
COPY ./WebApi/*.csproj ./
RUN dotnet restore

# Copy everything else and build
COPY ./WebApi ./
RUN dotnet publish -c Release -o out

# Build runtime image, following microsoft doc dockerfile instructions
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "WebApi.dll"]