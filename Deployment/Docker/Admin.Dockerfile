# our base image will be the .net core 3.1 sdk
FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build-env
WORKDIR /app

# Copy csproj and restore as distinct layers, like package.json in node projects
# You can see the directory expected of WebApi when looking into Web_Admin.csproj, 
# so we must follow that project structure in container
COPY ./NWBA_Web_Admin/NWBA_Web_Admin.csproj ./
COPY ./WebApi/WebApi.csproj ../WebApi/ 
RUN dotnet restore ./NWBA_Web_Admin.csproj
RUN dotnet restore ../WebApi/WebApi.csproj

# Copy everything else and build
# Don't publish api since it's published in another container amd will be exposed using network.
# Api still needs to be copied here for dependencies since originally this was one solution.
COPY ./NWBA_Web_Admin ./
COPY ./WebApi ../WebApi/
RUN dotnet publish ./NWBA_Web_Admin.sln -c Release -o out

# Build runtime image, following microsoft doc dockerfile instructions
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1
WORKDIR /app
COPY --from=build-env /app/out .
ENTRYPOINT ["dotnet", "NWBA_Web_Admin.dll"]