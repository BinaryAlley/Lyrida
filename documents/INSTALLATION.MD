# Installation 

Lyrida can be ran in two modes: either as docker containers (recommended) or in standalone mode.

## Requirements

Before you begin the installation of Lyrida, please ensure your system meets the following prerequisites:

### General Requirements

- **Relational Database**: A MariaDB database instance. MySQL might also work, but it is untested. The database must be accessible by the application, either as a local instance, a remote server, or a containerized service.

### For Standalone Setup

- **.NET 7 SDK**: This application is built with .NET 7, requiring the .NET 7 SDK. Download it from the [official .NET website](https://dotnet.microsoft.com/download/dotnet/7.0).
- **Operating System**: The application supports cross-platform deployment. Your operating system should be one of the following:
  - Linux distribution that supports .NET 7 (e.g., Debian 10, Ubuntu 20.04, Fedora 33)
  - Windows 10 or later
  - macOS (latest version)
- **ASP.NET Core Runtime**: Necessary to run the application, included in the .NET 7 SDK.

### For Docker Setup (recommended)

- **Docker**: You must have Docker installed to use the containerized version of the application. The latest version can be found on the [official Docker website](https://www.docker.com/get-started).
- **Docker Compose**: For orchestrating multi-container setups (optional), Docker Compose is necessary and comes included with Docker Desktop on Windows and macOS. It can also be found [here](https://docs.docker.com/compose/install/).

### Hardware Requirements

- **Processor**: 1 GHz or faster.
- **RAM**: 2 GB or more.
- **Disk Space**: Minimum of 1 GB available for the application, with additional space for the SQL database, if hosted on the same server.

All set? Proceed to the installation instructions provided below to get started.

## Configuration

### For Standalone Setup

#### appsettings.json

For the standalone version of the application, you need to configure the `appsettings.json` file for the server and the client app, respectively. Below are the templates you need to modify with your settings:

- **Server `appsettings.json`**:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "JwtSettings": {
    "SecretKey": "YourSecretKey",
    "ExpiryMinutes": 60,
    "Issuer": "YourIssuer",
    "Audience": "YourAudience"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=your-database-server;Port=your-database-port;Database=your-database-name;Uid=your-username;Pwd=your-password;Convert Zero Datetime=True;Allow User Variables=True;IgnoreCommandTransaction=True;"
  }
}
```

- **Client `appsettings.json`**:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ServerPath": "http://localhost",
  "ServerPort": 5000
}
```

Replace the placeholder values with your actual configuration settings. For production environments, it is recommended to use the `appsettings.Production.json` file.

### For Docker Setup 

With Docker, the configuration is managed through environment variables in the docker-compose.yml file. Here's a sample setup:

```yaml
version: '3.4'
services:
  lyrida-server:
    image: lyrida-server:latest
    container_name: lyrida-server
    environment:
      - ASPNETCORE_URLS=http://+:18137
      - ASPNETCORE_ENVIRONMENT=Production
      - JWT_SECRET_KEY=YourSecretKey
      - DATABASE_HOST=your-database-host
      - DATABASE_PORT=your-database-port
      - DATABASE_NAME=your-database-name
      - DATABASE_USER=your-database-user
      - DATABASE_PASSWORD=your-database-password
    ports:
      - 18137:18137
    volumes:
      - ./logs/server:/app/Logs
      - /path/to/your/files:/path/inside/container
    networks:
      - lyrida-network

  lyrida-client:
    image: lyrida-client:latest
    container_name: lyrida-client
    environment:
      - ASPNETCORE_URLS=http://+:18138
      - ASPNETCORE_ENVIRONMENT=Production
    ports:
      - 18138:18138
    volumes:
      - ./logs/client:/app/Logs
    networks:
      - lyrida-network
    depends_on:
      - lyrida-server

networks:
  lyrida-network
```

Replace the placeholder values (denoted by "your-*") with your actual data. For file paths, mount the directories you wish to access through the web explorer by specifying the paths on the host and in the container.

**Note on Docker Networks**:

If the Docker network specified in the `docker-compose.yml` file has already been created, you should use the `external: true` setting. This tells Docker Compose to use the existing network instead of creating a new one.

Here's how you would specify an existing network, at the end of the compose file:

```yaml
networks:
  lyrida-network:
    external: true
```

### Configuring JWT Secret Key

The option `JwtSettings:SecretKey` in the `appsettings.json` file or `JWT_SECRET_KEY=` in the Docker Compose is used to sign and verify JWT tokens issued by the application. It is crucial that this key is kept secret and is randomly generated to ensure the security of the tokens.

To generate a secure JWT secret key, you can use a variety of methods. For example, on a Unix/Linux system, you can use the following command to generate a 256-bit key:

```shell
openssl rand -base64 32
```

Or, for a more simple approach, you can use an online tool that generates random strings, ensuring that it is of sufficient length (at least 32 characters) and complexity.

***Important***: NEVER expose your JWT secret key in public repositories or share it with unauthorized individuals. Treat it as you would any sensitive password!

For the `Issuer` and `Audience` fields in the `JwtSettings`, you can use values that are meaningful to your application, such as the name of your company or the application itself. These don't have to be complicated strings; simple, unique identifiers like "MyAppIssuer" and "MyAppAudience" will work.

#### Example Server `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Error",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "JwtSettings": {
    "SecretKey": "ds1/Ly$hFHj4-(rs^Qq.M/@sP90%j@Ma",
    "ExpiryMinutes": 60,
    "Issuer": "Lyrida",
    "Audience": "Lyrida"
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Port=3306;Database=lyrida;Uid=lyrida;Pwd='&p75L&3QUi(zSHYX{#j7O+[ZZ50{b+';Convert Zero Datetime=True;Allow User Variables=True;IgnoreCommandTransaction=True;"
  }
}
```

#### Example Client `appsettings.json`:

```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Error",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ServerPath": "http://localhost",
  "ServerPort": 18137
}
```

#### Example Docker Compose:

```yaml
version: '3.4'
services:
  lyrida-server:
    image: lyrida-server:latest
    container_name: lyrida-server
    restart: always
    environment:
      - ASPNETCORE_URLS=http://+:18137
      - ASPNETCORE_ENVIRONMENT=Production
      - JWT_SECRET_KEY="ds1/Ly$hFHj4-(rs^Qq.M/@sP90%j@Ma"
      - DATABASE_HOST=mariadb
      - DATABASE_PORT=3306
      - DATABASE_NAME=lyrida
      - DATABASE_USER=lyrida
      - DATABASE_PASSWORD="&p75L&3QUi(zSHYX{#j7O+[ZZ50{b+"
    ports:
      - 18137:18137
    volumes:
      - ./logs/server:/app/Logs
      - /home/JohnDoe/Downloads:/Downloads
    networks:
      - lyrida-network

  lyrida-client:
    image: lyrida-client:latest
    container_name: lyrida-client
    restart: always
    environment:
      - ASPNETCORE_URLS=http://+:18138
      - ASPNETCORE_ENVIRONMENT=Production
    ports:
      - 18138:18138
    volumes:
      - ./logs/client:/app/Logs
    networks:
      - lyrida-network
    depends_on:
      - lyrida-server

networks:
  lyrida-network:
    external: true
```

If you don't have an externally managed SQL server, or want to run one just for Lyrida, you can make the database part of the docker compose too, as an extra container. Example usage:

```yaml
version: '3.4'
services:
  mariadb:
    image: 'mariadb:latest'
    container_name: mariadb
    restart: always
    ports:
      - 3306:3306
    environment:
      - MARIADB_ROOT_PASSWORD="b(*h!4pQP+%(^lY^dhU(^ty#nNWRhY"
      - MYSQL_DATABASE=lyrida
      - MYSQL_USER=lyrida
      - MYSQL_PASSWORD="&p75L&3QUi(zSHYX{#j7O+[ZZ50{b+"
    volumes:
      - ./data:/var/lib/mysql
    networks:
      - lyrida-network

  lyrida-server:
    image: lyrida-server:latest
    container_name: lyrida-server
    restart: always
    environment:
      - ASPNETCORE_URLS=http://+:18137
      - ASPNETCORE_ENVIRONMENT=Production
      - JWT_SECRET_KEY="ds1/Ly$hFHj4-(rs^Qq.M/@sP90%j@Ma"
      - DATABASE_HOST=mariadb
      - DATABASE_PORT=3306
      - DATABASE_NAME=lyrida
      - DATABASE_USER=lyrida
      - DATABASE_PASSWORD="&p75L&3QUi(zSHYX{#j7O+[ZZ50{b+"
    ports:
      - 18137:18137
    volumes:
      - ./logs/server:/app/Logs
      - /home/JohnDoe/Downloads:/Downloads
    networks:
      - lyrida-network
    depends_on:
      - mariadb

  lyrida-client:
    image: lyrida-client:latest
    container_name: lyrida-client
    restart: always
    environment:
      - ASPNETCORE_URLS=http://+:18138
      - ASPNETCORE_ENVIRONMENT=Production
    ports:
      - 18138:18138
    volumes:
      - ./logs/client:/app/Logs
    networks:
      - lyrida-network
    depends_on:
      - lyrida-server

networks:
  lyrida-network:
    external: true
```

## Running the Application in standalone mode

Once you've configured your application, you're ready to run it. The steps vary slightly between Windows and Unix-based systems.

### Running on Unix-based Systems

On Unix-based systems, use the `dotnet` command to run the provided DLL files.

To run the server application:

```shell
dotnet Lyrida.Api.dll
```

For the client application:

```
dotnet Lyrida.UI.dll
```

Make sure you're in the directory that contains the DLL files before running the commands. Also, ensure that the .dll files are marked as executable if necessary, using ```chmod +x Lyrida.Api.dll``` and ```chmod +x Lyrida.UI.dll``` commands.

### Running on Windows

Navigate to the directories containing the `Lyrida.Api.exe` and `Lyrida.UI.exe` executable files and execute them, in this order (the client expects the server to be already running).

## Running the application with docker

If you're using Docker, the application can be started using the `docker-compose` command.

Navigate to the directory containing your `docker-compose.yml` file and execute the following command:

```shell
docker-compose up -d
```

This will start all services defined in your `docker-compose.yml` file in detached mode, allowing them to run in the background.

To stop the application, run:

```shell
docker-compose down
```

When the server and the client apps are running, either as standalone or docker containers, you can visit `http://localhost:client-port-you-chose` in your browser, and the application should be initialized. 

You can now proceed to [Usage](./USAGE.MD).