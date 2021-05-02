FROM mcr.microsoft.com/dotnet/sdk:5.0 AS dotnet-builder
WORKDIR /app
COPY . /app
RUN dotnet restore FlashMEMO-backend.sln
RUN dotnet publish -c Release -o /app/publish FlashMEMO-backend.sln

### Estágio 2 - Subir a aplicação através dos binários ###
FROM mcr.microsoft.com/dotnet/aspnet:5.0-focal
WORKDIR /app
EXPOSE 80
EXPOSE 443
COPY --from=dotnet-builder /app/publish .
ENTRYPOINT ["dotnet", "API.dll"]