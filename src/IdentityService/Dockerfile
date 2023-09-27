FROM mcr.microsoft.com/dotnet/sdk:7.0 as build
WORKDIR /app
EXPOSE 80

COPY src/IdentityService/IdentityService.csproj src/IdentityService/IdentityService.csproj

# Restore packages
RUN dotnet restore src/IdentityService/IdentityService.csproj

# Copy the app folders over
COPY src/IdentityService src/IdentityService

WORKDIR /app/src/IdentityService

RUN dotnet publish -c Release -o /app/src/out

# build runtime image
FROM mcr.microsoft.com/dotnet/aspnet:7.0

WORKDIR /app

COPY --from=build /app/src/out . 
ENTRYPOINT [ "dotnet", "IdentityService.dll" ]