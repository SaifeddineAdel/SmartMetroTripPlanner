# Smart Metro Trip Planner

A metro trip-planning backend built with **ASP.NET Core and .NET 10**. The system manages metro network data and calculates routes between stations, including line transfers, estimated journey time, and ticket fare.

The project follows a microservices architecture and is fully containerized using **Docker and Docker Compose**, providing a consistent way to build, configure, and run the entire system.

## Key Features

- **Microservices Architecture** — Separate `MetroNetwork` and `TripPlanning` services communicating through REST APIs.
- **API Gateway** — Centralized entry point for routing client requests using YARP (Yet Another Reverse Proxy).
- **JWT Authentication** — Token-based authentication using JSON Web Tokens.
- **Refresh Tokens** — Refresh-token flow for obtaining new access tokens.
- **Clean Architecture** — Separation of concerns across API, Application, Domain, and Infrastructure layers.
- **Domain-Driven Design (DDD)** — Domain modeling and separation of business logic from infrastructure concerns.
- **SOLID Principles and Clean Code** — Maintainable code, clear responsibilities, and separation of concerns.
- **Graph-Based Route Planning** — Graph modeling and priority-queue-based shortest-path search.
- **Multi-Criteria Route Optimization** — Minimizes the number of stations first, then minimizes line transfers.
- **Centralized Exception Handling** — Custom exceptions, global exception handling, and standardized Problem Details responses.
- **Database Design** — SQL Server and Entity Framework Core with relational constraints and soft deletion.
- **Dependency Injection** — Constructor injection and typed `HttpClient` for inter-service communication.
- **Swagger** — API documentation and endpoint testing.
- **Docker Containerization** — Dockerized APIs and SQL Server, managed through Docker Compose.
- **Multi-Stage Docker Builds** — Separate .NET SDK build stages from lightweight ASP.NET runtime images.
- **Container Networking** — Internal service-to-service communication using Docker DNS and container names.
- **Database Persistence** — Docker volumes to preserve SQL Server data across container recreation.
- **Environment-Based Configuration** — Runtime configuration and secrets managed through environment variables.

## Technology Stack

**Backend**
- C#
- .NET 10
- ASP.NET Core Web API

**Architecture and Design**
- Microservices Architecture
- API Gateway with YARP
- Clean Architecture
- Domain-Driven Design (DDD)
- Feature-Based Organization
- SOLID Principles
- Clean Code

**Authentication**
- JWT Authentication
- Access Tokens
- Refresh Tokens

**API Development**
- RESTful APIs
- DTOs
- Swagger
- Typed HttpClient

**Database**
- SQL Server 2022 Developer
- Entity Framework Core
- Relational Data Modeling
- Soft Deletion

**Algorithms**
- Graph Modeling
- Priority Queue
- Lexicographic Priority Ordering
- Shortest-Path Search
- Route Reconstruction
- Transfer Identification

**Containerization and DevOps**
- Docker
- Dockerfiles
- Multi-Stage Builds
- Docker Compose
- Docker Networking
- Docker Volumes
- Container Health Checks
- Environment Variables
- `.dockerignore`

**Tools**
- Git
- GitHub
- Visual Studio

## Architecture

The solution consists of an API Gateway and two backend microservices, with each service following Clean Architecture.

```text
SmartMetroTripPlanner/
├── services/
│   ├── ApiGateway.Api/
│   │
│   ├── MetroNetwork/
│   │   └── src/
│   │       ├── MetroNetwork.Api/
│   │       ├── MetroNetwork.Application/
│   │       ├── MetroNetwork.Domain/
│   │       └── MetroNetwork.Infrastructure/
│   │
│   └── TripPlanning/
│       └── src/
│           ├── TripPlanning.Api/
│           ├── TripPlanning.Application/
│           ├── TripPlanning.Domain/
│           └── TripPlanning.Infrastructure/
│
└── README.md
```

### API Gateway

The API Gateway uses YARP to provide a centralized entry point for incoming requests and forward them to the appropriate backend service according to configured routes and clusters.

### MetroNetwork Service

Responsible for managing metro network data, including stations, metro lines, and their relationships.

Responsibilities include:
- Retrieving and managing stations and metro lines.
- Maintaining relationships between stations and metro lines.
- Providing REST APIs for network data.
- Managing relational data using Entity Framework Core and SQL Server.
- Supporting soft deletion for stations.

### TripPlanning Service

Responsible for calculating journeys between metro stations.

Responsibilities include:
- Retrieving network data from `MetroNetwork` through HTTP APIs.
- Building an in-memory graph representing metro connections.
- Finding routes using a priority-queue-based search algorithm.
- Identifying metro lines and transfer stations.
- Calculating estimated journey time and ticket fare.
- Returning journey details through REST APIs.

`TripPlanning` does not maintain a separate database. It retrieves the required metro network data from `MetroNetwork` through synchronous HTTP communication.

## Authentication

The application uses JWT-based authentication with access tokens and refresh tokens.

- **Access Token** — Used to authenticate requests to protected endpoints.
- **Refresh Token** — Used to obtain a new access token through the implemented refresh-token flow.
- **Authorization** — Protects endpoints according to the application's authentication and authorization configuration.

## Route Planning Algorithm

The metro network is modeled as a graph:

- **Vertices** represent metro stations.
- **Edges** represent connections between stations.
- **Search State** consists of a station ID and the metro line used to reach it.

The `MetroPathFinder` uses a **priority-queue-based, lexicographic shortest-path search**.

Each candidate state is evaluated using two criteria, in this order:

1. **Minimum number of stations** — The primary optimization criterion.
2. **Minimum number of line transfers** — The secondary criterion when station counts are equal.

The algorithm uses a `PriorityQueue` to process states in priority order, a distance dictionary to track the best known priority for each state, and previous-state tracking to reconstruct the resulting path.

### Route Planning Process

1. Build the in-memory metro graph.
2. Initialize the search from the starting station.
3. Explore connections in priority order.
4. Prefer routes with fewer stations.
5. When station counts are equal, prefer routes with fewer transfers.
6. Reconstruct the selected path.
7. Group route segments by metro line and identify transfers.
8. Calculate the estimated journey time and ticket fare.

**Note:** The route-selection algorithm does not minimize journey time directly. Travel time and fare are calculated separately from the selected route.

## Dockerization and Containerization

The entire application is containerized using **Docker** and managed through **Docker Compose**.

Each API runs in its own container, while SQL Server runs in a separate container. All containers communicate through a dedicated Docker network.

This setup provides a consistent runtime environment and allows the complete system to be built and started with a single command.

### Docker Architecture Overview

The Docker environment consists of four main containers:

| Container | Responsibility |
|---|---|
| `metro-gateway` | Main entry point for client requests and YARP reverse proxy |
| `metro-api` | Manages stations, metro lines, and network data |
| `trip-api` | Calculates routes, transfers, journey time, and ticket fare |
| `metro-sql` | SQL Server 2022 database server |

Each API has its own Dockerfile and is built using a **Multi-Stage Build** approach.

### Dockerfiles and Multi-Stage Builds

A separate Dockerfile was created for each API:

- `MetroNetwork API`
- `TripPlanning API`
- `API Gateway`

The Dockerfiles use two main stages:

**1. Build Stage**
- Uses the `.NET 10 SDK` image.
- Restores project dependencies using `dotnet restore`.
- Compiles and publishes the application using `dotnet publish`.

**2. Final Runtime Stage**
- Uses the `.NET 10 ASP.NET Runtime` image.
- Copies the published application from the build stage.
- Exposes container port `8080`.
- Defines the application startup command using `ENTRYPOINT`.

This approach keeps the final runtime image separate from the SDK and build environment.

Each service also has a `.dockerignore` file to exclude unnecessary files and directories such as:

- `bin/`
- `obj/`
- `.vs/`

This reduces unnecessary build-context files and avoids copying local development artifacts into the Docker build process.

### SQL Server Container

The project uses SQL Server 2022 Developer running in a dedicated Docker container.

**Image:**

```text
mcr.microsoft.com/mssql/server:2022-latest
```

**Container name:** `metro-sql`

**Port mapping:** `1433:1433`

The existing `MetroNetworkDb` database was migrated from the local SQL Server instance to the Docker SQL Server container using **Backup and Restore**.

This allows the application to use the database inside the containerized environment instead of depending on a locally installed SQL Server instance.

### Docker Volume and Database Persistence

A Docker volume named `metronetwork-data` is used to persist SQL Server data.

```text
metronetwork-data:/var/opt/mssql
```

The volume is mounted at SQL Server's data directory:

```text
/var/opt/mssql
```

This ensures that database files remain available even when the SQL Server container is stopped, removed, or recreated.

The volume is configured as an external Docker resource so the existing database data can be retained independently of the Compose container lifecycle.

### Docker Networking and Internal Communication

A dedicated Docker network named `metronetwork-network` connects all four containers.

The services communicate internally using **Docker DNS and container names**, rather than `localhost`.

Internal communication:

| Source | Destination | Internal Address |
|---|---|---|
| TripPlanning API | MetroNetwork API | `http://metro-api:8080/` |
| API Gateway | MetroNetwork API | `http://metro-api:8080/` |
| API Gateway | TripPlanning API | `http://trip-api:8080/` |
| MetroNetwork API | SQL Server | `metro-sql,1433` |

**Important:** Inside a container, `localhost` refers to that same container, not another service. Docker DNS allows containers on the same network to resolve each other using their container names.

`TripPlanning` communicates synchronously with `MetroNetwork` through HTTP Client and does not have its own database.

### Environment Variables and Configuration

Runtime configuration is managed through environment variables using ASP.NET Core's configuration system.

A `.env` file is used to provide environment-specific settings to Docker Compose, including connection strings, JWT configuration, and service destinations.

Examples of configuration keys include:

- `ConnectionStrings__DefaultConnection`
- `Jwt__SecretKey`
- `Services__MetroNetwork__BaseUrl`
- YARP Reverse Proxy destination settings.

The double underscore (`__`) is used by ASP.NET Core to represent configuration hierarchy. For example:

```text
Jwt__SecretKey
```

maps to:

```text
Jwt:SecretKey
```

#### Setting Up the Environment

The `.env` file is intentionally excluded from Git using `.gitignore` to prevent secrets and environment-specific configuration from being committed.

Before starting the application:

1. Create a `.env` file in the project root, alongside `compose.yaml`.
2. Add the environment variables required by the Compose configuration.
3. Replace the placeholder values with your own local configuration.
4. Make sure the YARP destination variables match the names referenced in `compose.yaml`.

Example structure:

```dotenv
# SQL Server
MSSQL_SA_PASSWORD=<YOUR_SQL_SERVER_PASSWORD>

# Database connection
ConnectionStrings__DefaultConnection=Server=metro-sql,1433;Database=MetroNetworkDb;User Id=sa;Password=<YOUR_SQL_SERVER_PASSWORD>;TrustServerCertificate=True

# JWT
Jwt__SecretKey=<YOUR_JWT_SECRET_KEY>

# Internal service URLs
Services__MetroNetwork__BaseUrl=http://metro-api:8080/

# YARP destination settings
# Add the destination environment variables
# referenced by your compose.yaml configuration.
```

**Security note:** The values above are placeholders, not working credentials. Never commit real database passwords, JWT secrets, or other sensitive environment variables to GitHub.

### Docker Compose

A `compose.yaml` file is located in the project root and manages the complete containerized environment.

It defines and coordinates the following four services:

- `metro-sql`
- `metro-api`
- `trip-api`
- `metro-gateway`

The Compose configuration includes:

- **Build Configuration** — Builds the API images from their respective Dockerfiles.
- **Environment Variables** — Supplies runtime configuration to the containers.
- **Port Mapping** — Exposes the required container ports to the host machine.
- **Docker Network** — Connects all services through `metronetwork-network`.
- **Docker Volume** — Persists SQL Server data through `metronetwork-data`.
- **Service Dependencies** — Uses `depends_on` to define service startup dependencies.
- **Health Check** — Uses `sqlcmd` to check SQL Server health.
- **Restart Policies** — Defines container restart behavior.

The existing Docker network and volume are configured as external resources. This allows the Compose project to use the existing network and database volume rather than creating new resources and unintentionally replacing the existing database storage.

### Building and Running the Application

Make sure Docker Desktop is running and the `.env` file has been configured before starting the application.

From the project root directory, run:

**Build images and start all services:**

```bash
docker compose up -d --build
```

This command builds the API images and starts the four containers in detached mode.

**Check container status:**

```bash
docker compose ps
```

**Follow logs for all services:**

```bash
docker compose logs -f
```

**View logs for a specific service:**

```bash
docker compose logs -f metro-api
```

You can replace `metro-api` with `trip-api`, `metro-gateway`, or `metro-sql`.

**Stop and remove the Compose containers:**

```bash
docker compose down
```

This stops and removes the containers created by the Compose project. The external Docker volume and network are kept independently, so the persisted database data is not removed by this command.

**Start the services again:**

```bash
docker compose up -d
```

### Application URLs and Port Mappings

The following ports are exposed on the host machine:

| Service | Host Port | Container Port |
|---|---:|---:|
| API Gateway | `8080` | `8080` |
| MetroNetwork API | `8081` | `8080` |
| TripPlanning API | `8082` | `8080` |
| SQL Server | `1433` | `1433` |

The API Gateway is the main entry point for client requests.

| Component | URL |
|---|---|
| API Gateway | `http://localhost:8080` |
| MetroNetwork API | `http://localhost:8081` |
| TripPlanning API | `http://localhost:8082` |
| SQL Server | `localhost,1433` |

The backend APIs are also exposed directly through their host ports for development and testing. In the normal client request flow, the API Gateway handles routing to the appropriate backend service.

## API Example

### Plan a Trip

Example request:

```http
POST /api/trips/plan
Content-Type: application/json
```

```json
{
  "startStationId": 1,
  "destinationStationId": 35
}
```

The response includes journey information supported by the API, such as:

- Start and destination stations.
- Route segments.
- Metro lines used.
- Transfer information.
- Total station count.
- Estimated travel time.
- Total ticket fare.

The API Gateway's external routes may differ from internal service routes according to the YARP configuration.

## Error Handling

The `TripPlanning` service uses custom application exceptions for common error scenarios:

- `BadRequestException` — Invalid request values.
- `NotFoundException` — Stations or routes that cannot be found.
- `ConflictException` — Conflicting operations.
- `ServiceUnavailableException` — An unavailable dependent service.

A global exception handler maps application exceptions to appropriate HTTP status codes and returns standardized Problem Details responses.

Unexpected errors are handled centrally without exposing internal exception details to API consumers.
