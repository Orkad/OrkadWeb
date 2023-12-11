
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build-env
WORKDIR /build

RUN curl -sL https://deb.nodesource.com/setup_18.x | bash - 
RUN apt-get install -y nodejs

# Copy everything
COPY . ./
# Restore as distinct layers
RUN dotnet restore OrkadWeb.Angular
# Build and publish a release
RUN dotnet publish OrkadWeb.Angular -c Release -o out

# Build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:8.0
WORKDIR /app
COPY --from=build-env /build/out .
ENTRYPOINT ["dotnet", "OrkadWeb.Angular.dll"]
EXPOSE 5000
