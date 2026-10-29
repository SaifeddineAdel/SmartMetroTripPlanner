# Smart Metro Trip Planner

A metro trip-planning backend built with **ASP.NET Core and .NET 10**. The system manages metro network data and calculates routes between stations, including line transfers, estimated journey time, and ticket fare.

## Key Features

- **Microservices Architecture** — Separate `MetroNetwork` and `TripPlanning` services communicating through REST APIs.
- **API Gateway** — Centralized entry point for routing client requests to backend services using YARP (Yet Another Reverse Proxy).
- **JWT Authentication** — Token-based authentication using JSON Web Tokens.
- **Refresh Tokens** — Refresh-token flow for obtaining new access tokens without requiring users to authenticate again.
- **Clean Architecture** — Separation of concerns across API, Application, Domain, and Infrastructure layers.
- **Domain-Driven Design (DDD)** — Domain modeling and separation of business logic from infrastructure concerns.
- **SOLID Principles and Clean Code** — Maintainable code, clear responsibilities, and separation of concerns.
- **Graph-Based Route Planning** — Graph modeling and priority-queue-based shortest-path search.
- **Multi-Criteria Route Optimization** — Minimizes the number of stations first, then minimizes line transfers.
- **Centralized Exception Handling** — Custom exceptions, global exception handling, and standardized Problem Details responses.
- **Database Design** — SQL Server and Entity Framework Core with relational constraints and soft deletion.
- **Dependency Injection** — Constructor injection and typed `HttpClient` for inter-service communication.
- **Swagger** — API documentation and endpoint testing.

## Technology Stack

### Backend
- C#
- .NET 10
- ASP.NET Core Web API

### Architecture and Design
- Microservices Architecture
- API Gateway with YARP
- Clean Architecture
- Domain-Driven Design (DDD)
- Feature-Based Organization
- SOLID Principles
- Clean Code

### Authentication
- JWT Authentication
- Access Tokens
- Refresh Tokens

### API Development
- RESTful APIs
- DTOs
- Swagger

### Database
- SQL Server
- Entity Framework Core
- Relational Data Modeling
- Soft Deletion

### Algorithms
- Graph Modeling
- Priority Queue
- Lexicographic Priority Ordering
- Shortest-Path Search
- Route Reconstruction
- Transfer Identification

### Tools
- Git
- GitHub
- Visual Studio

## Architecture

The solution contains an API Gateway and two backend services. Each backend service follows Clean Architecture.

```text
SmartMetroTripPlanner/
├── ApiGateway.Api/
│
├── MetroNetwork.Api/
├── MetroNetwork.Application/
├── MetroNetwork.Domain/
├── MetroNetwork.Infrastructure/
│
├── TripPlanning.Api/
├── TripPlanning.Application/
├── TripPlanning.Domain/
├── TripPlanning.Infrastructure/
│
└── README.md
```

The directory tree is a logical overview. Refer to the actual repository structure for the exact project locations.

### API Gateway

The API Gateway uses YARP to provide a centralized entry point for incoming requests and forward them to the appropriate backend service according to configured routes and clusters.

### MetroNetwork Service

Responsible for managing metro network data, including stations, metro lines, and their relationships.

Responsibilities include:
- Retrieving and managing stations and metro lines.
- Maintaining relationships between stations and metro lines.
- Providing REST APIs for network data.
- Persisting data through Entity Framework Core and SQL Server.
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

## Authentication

The application uses JWT-based authentication with access tokens and refresh tokens.

- **Access Token** — Used to authenticate requests to protected endpoints.
- **Refresh Token** — Used to obtain a new access token according to the implemented refresh-token flow.
- **Protected Endpoints** — Require valid authentication and the appropriate authorization.

Authentication endpoints, token expiration, refresh-token storage, and revocation behavior depend on the actual implementation.

## Route Planning Algorithm

The metro network is modeled as a graph:

- **Vertices** represent metro stations.
- **Edges** represent connections between stations.
- **Search State** consists of a station ID and the metro line used to reach it.

The `MetroPathFinder` uses a **priority-queue-based, lexicographic shortest-path search**.

Each candidate state is evaluated using two criteria, in this order:

1. **Minimum number of stations** — The primary optimization criterion.
2. **Minimum number of line transfers** — The secondary criterion when station counts are equal.

The algorithm uses a `PriorityQueue`, a distance dictionary to track the best known priority for each search state, and previous-state tracking to reconstruct the resulting route.

### Route Planning Process

1. Load the metro network.
2. Build the in-memory graph.
3. Initialize the search from the starting station.
4. Explore candidate connections in priority order.
5. Prefer routes with fewer stations.
6. When station counts are equal, prefer routes with fewer transfers.
7. Reconstruct the selected path.
8. Group route segments by metro line and identify transfers.
9. Calculate the estimated journey time and ticket fare.

**Important:** This algorithm does not optimize journey time directly. Travel time and fare are calculated separately from the selected route.

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

The response includes the journey information supported by the API, such as:

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

## Prerequisites

- .NET 10 SDK
- SQL Server
- Visual Studio or another .NET-compatible IDE

## Getting Started

### 1. Clone the Repository

```bash
git clone https://github.com/SaifeddineAdel/SmartMetroTripPlanner.git
cd SmartMetroTripPlanner
```

### 2. Configure the Database

Configure the SQL Server connection string in the appropriate `MetroNetwork` configuration.

Ensure the database schema exists and apply the required Entity Framework Core migrations, if available.

### 3. Configure Service URLs

Configure `Services:MetroNetwork:BaseUrl` in the `TripPlanning` service to point to the running `MetroNetwork` API.

Configure the API Gateway's `ReverseProxy` section with the appropriate routes and clusters.

### 4. Configure Authentication

Configure the JWT settings and refresh-token behavior required by the authentication implementation.

Keep secrets, passwords, and sensitive connection details out of source control. Use .NET User Secrets for local development or an appropriate secret-management solution for deployment.

### 5. Run the Services

Run the `MetroNetwork` API, `TripPlanning` API, and API Gateway using their configured launch profiles and ports.

Ensure the gateway routes match the actual service endpoints.

### 6. Explore the APIs with Swagger

Open the Swagger UI configured for each service or gateway, depending on the project's setup.

Use Swagger to inspect endpoints, submit requests, and test API responses. Protected endpoints may require authentication.

## Testing

The main API flows have been manually tested using Swagger, where applicable.

Automated unit and integration tests can be added to verify domain behavior, route calculations, authentication, and communication between services.

## Notes

- Update database connection strings and service URLs for your local environment.
- Ensure YARP routes and clusters match the actual service endpoints.
- Route selection prioritizes the minimum number of stations, then the minimum number of transfers.
- Estimated travel time and ticket fare are calculated separately from route selection.
- Never commit JWT secrets, passwords, or other sensitive configuration values.
