#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["FlashMEMO-backed-API/FlashMEMO-backed-API.csproj", "FlashMEMO-backed-API/"]
RUN dotnet restore "FlashMEMO-backed-API/FlashMEMO-backed-API.csproj"
COPY . .
WORKDIR "/src/FlashMEMO-backed-API"
RUN dotnet build "FlashMEMO-backed-API.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "FlashMEMO-backed-API.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FlashMEMO-backed-API.dll"]