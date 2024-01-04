#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["PharmaRep.WebAPI/_PharmaRep.WebAPI.csproj", "PharmaRep.WebAPI/"]
COPY ["PharmaRep.Domain/PharmaRep.Domain.csproj", "PharmaRep.Domain/"]
COPY ["PharmaRep.Infra/PharmaRep.Infra.csproj", "PharmaRep.Infra/"]
COPY ["PharmaRep.Application/PharmaRep.Application.csproj", "PharmaRep.Application/"]
RUN dotnet restore "./PharmaRep.WebAPI/./_PharmaRep.WebAPI.csproj"
COPY . .
WORKDIR "/src/PharmaRep.WebAPI"
RUN dotnet build "./_PharmaRep.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./_PharmaRep.WebAPI.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "_PharmaRep.WebAPI.dll"]