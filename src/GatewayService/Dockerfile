FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /app
EXPOSE 80

COPY src/GatewayService/GatewayService.csproj src/GatewayService/GatewayService.csproj

# Restore packages
RUN dotnet restore src/GatewayService/GatewayService.csproj

# Copy the app folders over
COPY src/GatewayService src/GatewayService

WORKDIR /app/src/GatewayService

RUN dotnet publish -c Release -o /app/src/out

# build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app

COPY --from=build /app/src/out . 
ENTRYPOINT [ "dotnet", "GatewayService.dll" ]