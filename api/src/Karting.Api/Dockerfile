# syntax=docker/dockerfile:1

FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ["api/src/Karting.Api/Karting.Api.csproj", "api/src/Karting.Api/"]
COPY ["api/src/Karting.Application/Karting.Application.csproj", "api/src/Karting.Application/"]
COPY ["api/src/Karting.Domain/Karting.Domain.csproj", "api/src/Karting.Domain/"]
COPY ["api/src/Karting.Infrastructure/Karting.Infrastructure.csproj", "api/src/Karting.Infrastructure/"]

RUN dotnet restore "api/src/Karting.Api/Karting.Api.csproj"

COPY . .
WORKDIR /src/api/src/Karting.Api
RUN dotnet publish "Karting.Api.csproj" -c Release -o /app/publish /p:UseAppHost=false

FROM mcr.microsoft.com/dotnet/aspnet:10.0 AS final
WORKDIR /app
EXPOSE 8080
ENV ASPNETCORE_URLS=http://+:8080
COPY --from=build /app/publish .
ENTRYPOINT ["dotnet", "Karting.Api.dll"]
