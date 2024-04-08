#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.
# Define build-time variables
ARG BUILD_CONFIGURATION=Release

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

#ENV DOTNET_CLI_DO_NOT_USE_MSBUILD_SERVER = 1
ARG SERVER_NAME
ARG DATABASE_NAME
ARG USERNAME
ARG PASSWORD
ENV SERVER_NAME = ${SERVER_NAME}
ENV DATABASE_NAME = ${DATABASE_NAME}
ENV USERNAME = ${USERNAME}
ENV PASSWORD = ${PASSWORD}

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["SpectaclesStack.Api/spectaclesStackServer.csproj", "."]
RUN dotnet restore "spectaclesStackServer.csproj"
COPY . .
WORKDIR /src
RUN dotnet build "SpectaclesStack.Api/spectaclesStackServer.csproj" -c $BUILD_CONFIGURATION /app/build 

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "SpectaclesStack.Api/spectaclesStackServer.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app 
COPY --from=publish /app/publish .

ENV ASPNETCORE_ENVIRONMENT=Development

# start running the application
ENTRYPOINT ["dotnet", "spectaclesStackServer.dll"] 