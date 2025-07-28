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
COPY ["Tajan.ProductService.API/Tajan.ProductService.API.csproj", "Tajan.ProductService.API/"]
COPY ["Tajan.ProductService.Application/Tajan.ProductService.Application.csproj", "Tajan.ProductService.Application/"]
COPY ["Tajan.ProductServcie.Domain/Tajan.ProductServcie.Domain.csproj", "Tajan.ProductServcie.Domain/"]
COPY ["Tajan.Standard.Domain/Tajan.Standard.Domain.csproj", "Tajan.Standard.Domain/"]
COPY ["Tajan.ProductService.Infrastructure/Tajan.ProductService.Infrastructure.csproj", "Tajan.ProductService.Infrastructure/"]
COPY ["Tajan.Standard.Presentation/Tajan.Standard.Presentation.csproj", "Tajan.Standard.Presentation/"]
RUN dotnet restore "./Tajan.ProductService.API/Tajan.ProductService.API.csproj"
COPY . .
WORKDIR "/src/Tajan.ProductService.API"
RUN dotnet build "./Tajan.ProductService.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tajan.ProductService.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tajan.ProductService.API.dll"]