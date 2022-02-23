#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:6.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build
WORKDIR /src
COPY ["MeterReadings.API/MeterReadings.API.csproj", "MeterReadings.API/"]
COPY ["MeterReadings.Services/MeterReadings.Services.csproj", "MeterReadings.Services/"]
COPY ["MeterReadings.Models/MeterReadings.Models.csproj", "MeterReadings.Models/"]
COPY ["MeterReadings.Data/MeterReadings.Data.csproj", "MeterReadings.Data/"]
RUN dotnet restore "MeterReadings.API/MeterReadings.API.csproj"
COPY . .
WORKDIR "/src/MeterReadings.API"
RUN dotnet build "MeterReadings.API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "MeterReadings.API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "MeterReadings.API.dll"]