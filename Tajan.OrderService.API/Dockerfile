# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS="http://+:8080"
EXPOSE 8080

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Tajan.OrderService.API/Tajan.OrderService.API.csproj", "Tajan.OrderService.API/"]
COPY ["Tajan.OrderService.Application/Tajan.OrderService.Application.csproj", "Tajan.OrderService.Application/"]
COPY ["Tajan.OrderService.Domain/Tajan.OrderService.Domain.csproj", "Tajan.OrderService.Domain/"]
COPY ["Tajan.Standard.Domain/Tajan.Standard.Domain.csproj", "Tajan.Standard.Domain/"]
COPY ["Tajan.Standard.Presentation/Tajan.Standard.Presentation.csproj", "Tajan.Standard.Presentation/"]
RUN dotnet restore "./Tajan.OrderService.API/Tajan.OrderService.API.csproj"
COPY . .
WORKDIR "/src/Tajan.OrderService.API"
RUN dotnet build "./Tajan.OrderService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tajan.OrderService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tajan.OrderService.API.dll"]