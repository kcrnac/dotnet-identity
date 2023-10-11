# dotnet-identity microservice

Repository contains one way of implementing token based authentication (JWT Bearer) in .NET 7 as microservices with NextJS application on frontend, all of them running inside Docker containers.

At the moment there are 5 services running in Docker

- _**postgres**_ - data store for membership
- _**identity-svc**_ - web api microservice that implements JWT Auth
- _**gateway-svc**_ - YARP based reverse proxy service that sits in front of other microservices (at the moment, only in front of identity-svc)
- _**web-app**_ - NextJS web application that acts as a frontend (but uses SSR and ServerActions which means it needs node.js to run)
- _**nginx-proxy**_ - proxy used to implement SSL

Application consist of following sections/features:

- Register new user (API only)
- Login with user's credentials (API / WebApp)
- Access authorized-only endpoints (API / WebApp)

From technical perspective _**IdentityService**_ project contains most of the "logic". It is a WebAPI project based on .NET 7 version that uses identity system with custom user & roles, in combination with Entity Framework Core. As previously mentioned, all membership data such as users, roles, and others, are stored to PostgreSQL database that runs on Docker. If you're interested, you can look more into details on following topics:

- [Service registrations](https://github.com/kcrnac/dotnet-identity/blob/main/src/IdentityService/Extensions/ServiceExtensions.cs), such as Identity, JWT Auth, custom ModelState error handling, EF migrations
- [TokenService](https://github.com/kcrnac/dotnet-identity/blob/main/src/IdentityService/Infrastructure/Services/TokenService.cs) that handles JWT token creation
- Custom [exception middleware](https://github.com/kcrnac/dotnet-identity/blob/main/src/IdentityService/Middleware/ExceptionMiddleware.cs)

## How do I run it locally on Docker?

1. Ensure you have [Docker Deskop](https://docs.docker.com/desktop/) installed on your machine and you switched to Linux containers once it's up & running.
2. Clone the repository to your local machine using your terminal, GitHub Desktop, or any other tool you prefer
   ```
   https://github.com/kcrnac/dotnet-identity.git
   ```
3. In order to access API & Web App you will need to provide an SSL certificate which you can create by first installing 'mkcert' from their [official repository](https://github.com/FiloSottile/mkcert). Once installed, run the command to install the local Certificate Authority (CA)
   ```
   mkcert -install
   ```
4. Next, create your own certificate and key file, replacing the one which came with this repository.

   1. Navigate to certificates folder using terminal

      ```
      cd certificates
      ```

   2. Create certificate and key using 'mkcert' tool

      ```
      mkcert -key-file dotnet-identity.com.key -cert-file dotnet-identity.com.crt  api.dotnet-identity.com app.dotnet-identity.com
      ```

5. Using terminal, VS Code, or any other tool you prefer, navigate to the root folder of the cloned repository on your local machine and run the following command that will build all of the services and create docker images
   ```
   docker compose build
   ```
6. Once build has completed successfully, start the services
   ```
   docker compose up -d
   ```

7. Update your host file in order to access the application by its domain name. If you're not sure how to do it, please follow this [guide](https://docs.rackspace.com/docs/modify-your-hosts-file).
   ```
   127.0.0.1 api.dotnet-identity.com app.dotnet-identity.com
   ```
8. Access the web app on https://app.dotnet-identity.com

### Disclaimer

Presented solution is in no way representation of best practice, but rather something I used as a playground for trying out different things.

### Credits

Repository was mostly inspired by [Carsties](https://github.com/TryCatchLearn/Carsties/tree/main) solution. I strongly suggest to everyone who's interested to learn more, to look into Neil's Udemy channel which offers easy to follow videos on topics such as Microservices, Docker, .NET Core, React, and others.

#### Additional resources

[JWT Authentication and role based authorization in .net 6/7](https://ravindradevrani.medium.com/net-7-jwt-authentication-and-role-based-authorization-5e5e56979b67)

[.NET 7 Web API 🔒 JWT Authentication and role-based Authorization](https://shahedbd.medium.com/net-7-web-api-jwt-authentication-and-role-based-authorization-f2ff81f69cd4)
