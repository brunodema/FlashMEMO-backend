# FlashMEMO-backend
# As of today (16/07/2022), the app requires the following additional files to be mounted in a '/config' folder in order to work properly:
# - /config/appsettings.Production.json
# - /config/apisettings.json
# - /config/dbsettings.json
# - /config/emailsettings.json

# Also, the following certificates must be mounted in the '/ssl' folder to enforce HTTPS in production environment:
# - /ssl/flashmemo.edu.pem
# - /ssl/flashmemo.edu-key.pem

# Template taken from here the official Microsoft documentation (can't paste link because text cannot be escaped)
FROM mcr.microsoft.com/dotnet/sdk:6.0 AS build-env
WORKDIR /app

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore
# Build and publish a release
RUN dotnet publish -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:6.0
WORKDIR /app
COPY --from=build-env /app/out .

RUN ls
RUN echo "hello there"

EXPOSE 80
EXPOSE 443

ENTRYPOINT ["dotnet", "API.dll"]