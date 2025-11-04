FROM mcr.microsoft.com/dotnet/aspnet:9.0 AS base
WORKDIR /app
ENV ASPNETCORE_URLS="http://+:80"
EXPOSE 80

FROM mcr.microsoft.com/dotnet/sdk:9.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Tajan.Identity.API/Tajan.Identity.API.csproj", "Tajan.Identity.API/"]
RUN dotnet restore "./Tajan.Identity.API/Tajan.Identity.API.csproj"
COPY . .
WORKDIR "/src/Tajan.Identity.API"
RUN dotnet build "./Tajan.Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Tajan.Identity.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Tajan.Identity.API.dll"]
