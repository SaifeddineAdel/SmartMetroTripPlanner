A metro trip-planning backend built with ASP.NET Core and .NET 10, designed to find routes between stations, identify line transfers, and calculate estimated journey time and fare.
- Microservices Architecture — Separate MetroNetwork and TripPlanning services communicating through REST APIs.
- Clean Architecture — Separation of concerns across API, Application, Domain, and Infrastructure layers.
- Domain-Driven Design (DDD) — Domain modeling and separation of domain logic from infrastructure concerns.
- Clean Code & SOLID Principles — Focus on readable, maintainable code, clear responsibilities, and separation of concerns.
- Graph Algorithms — Graph modeling and Breadth-First Search (BFS) for metro route finding.
- Centralized Exception Handling — Custom exceptions, global exception handling, and standardized Problem Details responses.
- Database Design — SQL Server and Entity Framework Core with relational constraints and soft deletion.
- Dependency Injection — Constructor injection and typed HttpClient for inter-service communication.
- Backend: C#, .NET 10, ASP.NET Core Web API
- Architecture: Microservices, Clean Architecture, Domain-Driven Design (DDD), Feature-Based Organization, SOLID Principles, Clean Code
- API Development: RESTful APIs, DTOs, Swagger
- Validation & Error Handling: Request Validation, Custom Exceptions, Global Exception Handling, Problem Details
- Database: SQL Server, Entity Framework Core, Relational Data Modeling
- Algorithms: Graphs, Breadth-First Search (BFS), Route Reconstruction
- Tools: Git, GitHub, Visual Studio
Responsible for managing metro network data, including stations, metro lines, and their relationships.
- REST APIs for retrieving and managing stations and metro lines.
- Entity Framework Core for database access.
- SQL Server for relational data storage.
- Soft deletion for stations.
Responsible for calculating journeys between metro stations.
- Retrieves network data from MetroNetwork through HTTP APIs.
- Builds an in-memory graph representing metro connections.
- Uses BFS to find a route between two stations.
- Groups route segments by metro line and identifies transfers.
- Calculates estimated journey time and fare.
- Uses centralized exception handling for consistent API error responses.
```
SmartMetroTripPlanner/
├── services/
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
Example request:
```
{
  "startStationId": 1,
  "destinationStationId": 35
}
```
The response includes the start and destination stations, route segments, metro lines, transfer information, total station count, estimated travel time, and total fare.
The TripPlanning service uses custom application exceptions to handle common error scenarios:
- BadRequestException — Invalid request values.
- NotFoundException — Stations or routes that cannot be found.
- ConflictException — Conflicting operations.
- ServiceUnavailableException — Unavailable dependent services.
A global exception handler maps application exceptions to appropriate HTTP status codes and returns standardized Problem Details responses. Unexpected errors are handled centrally without exposing internal exception details to API consumers.
- .NET 10 SDK
- SQL Server
- Visual Studio or another .NET-compatible IDE
1. Clone the repository:
   ```
   git clone https://github.com/SaifeddineAdel/SmartMetroTripPlanner.git
   ```
2. Configure the SQL Server connection string in the MetroNetwork configuration.
3. Ensure the database schema exists and apply the required migrations, if available.
4. Run the MetroNetwork API.
5. Configure Services:MetroNetwork:BaseUrl in TripPlanning to point to the running MetroNetwork API.
6. Run the TripPlanning API.
7. Open Swagger UI for each service to explore and test the endpoints.
Note: Database connection strings and service URLs may need to be adjusted for your local environment.
The metro network is modeled as a graph where stations are vertices and connections between consecutive stations are edges.
The TripPlanning service uses Breadth-First Search (BFS) to find a route in the unweighted graph. BFS finds a route with the minimum number of edges, but it does not necessarily minimize travel time or the number of line transfers.
The MetroNetwork and TripPlanning services have been implemented, and their main API flows have been manually tested using Swagger.
