# Smart Metro Trip Planner

A scalable, microservices-based Metro Trip Planning platform built with **.NET 10**, designed to help users find optimal metro routes while demonstrating clean architecture, software engineering principles, and performance-oriented backend development.

## Project Vision

Smart Metro Trip Planner is designed with a production-oriented architecture, focusing on **maintainability, scalability, performance, and separation of responsibilities**.

The system provides a strong foundation for future growth, allowing individual services to evolve and scale independently as business requirements increase.

Its architecture is designed to support:
- Independent scaling of microservices.
- Extending trip-planning capabilities without tightly coupling services.
- Efficient data access through caching and in-memory processing.
- Future infrastructure and deployment improvements.
- Maintainable and testable business logic.

Actual production deployment would additionally require operational practices such as monitoring, logging, security hardening, resilience, and load testing.

## How the System Works

Smart Metro Trip Planner allows users to plan their metro journeys without requiring registration or login.

### User Journey

1. **Select Stations:** The user selects a starting station and a destination station.
2. **Send Trip Request:** The request is sent to the API Gateway, which routes it to the TripPlanning service.
3. **Find the Optimal Route:** TripPlanning uses its in-memory graph representation of the metro network to find the best available route.
4. **Calculate Trip Details:** The system calculates the estimated travel time, number of stations, transfers, and ticket fare.
5. **Display Results:** The user receives the recommended journey, including the ordered stations, metro lines, transfer points, estimated duration, and ticket price.

The route selection prioritizes the minimum number of stations, followed by the minimum number of transfers.

### Administration & Authentication

Authentication is required only for administrators responsible for managing the metro network.

Administrators can securely log in using **ASP.NET Core Identity, JWT Access Tokens, and Refresh Tokens** to access protected management endpoints.

Administrative responsibilities include:
- Managing metro stations.
- Managing metro lines.
- Managing station-to-line relationships.

Regular users can access trip-planning functionality without creating an account or authenticating.

## Architecture Overview

The project follows a **Microservices Architecture** with clearly separated responsibilities.

### Services

**1. API Gateway**
- Built with YARP.
- Acts as the single entry point for client requests.
- Routes requests to the appropriate microservice.

**2. MetroNetwork Service**
- Owns and manages metro network data.
- Handles Stations, Metro Lines, and Line Stations.
- Uses SQL Server as its primary persistent data store.
- Publishes the latest network snapshot to Redis through a background process.
- Provides protected administrative endpoints for managing metro network data.

**3. TripPlanning Service**
- Responsible for calculating optimal metro journeys.
- Retrieves the latest metro network snapshot from Redis.
- Builds and maintains an in-memory graph.
- Calculates routes, transfers, estimated travel time, and ticket fares.

## Data Synchronization & Caching

Redis stores the latest metro network snapshot, allowing TripPlanning to access network data without repeatedly querying the MetroNetwork service or its database.

A .NET `BackgroundService` handles publishing updated network snapshots to Redis. TripPlanning loads the snapshot and builds its in-memory graph, which is then used to process trip requests efficiently.

This approach separates data ownership from route calculation and reduces unnecessary database access and inter-service communication during trip requests.

## Clean Architecture & Software Engineering

The project applies Clean Architecture principles to keep business logic independent from infrastructure and external frameworks.

### Architectural Principles

- **Separation of Concerns:** Each service and layer has a clearly defined responsibility.
- **Dependency Inversion:** Business logic depends on abstractions rather than infrastructure implementations.
- **Domain-Centric Design:** Core business rules are separated from database and API concerns.
- **Feature-Based Organization:** Related functionality is grouped together for better maintainability.
- **Dependency Injection:** Dependencies are managed through the built-in .NET DI container.
- **DTOs & Validation:** API contracts are separated from domain entities, with request validation handled independently.

### Software Engineering Practices

- SOLID principles.
- Clean Code and maintainable code organization.
- Reusable abstractions and dependency management.
- Centralized exception handling.
- Asynchronous programming with `async/await`.
- Automated testing.
- EF Core migrations and structured database persistence.

## Route Planning Engine

The TripPlanning service uses graph-based algorithms to calculate optimal metro journeys.

Key capabilities:
- Finds routes between source and destination stations.
- Supports movement in both directions.
- Handles transfers between metro lines.
- Prioritizes the minimum number of stations, then the minimum number of transfers.
- Calculates estimated journey duration.
- Calculates ticket fares based on the number of stations.

The route-planning logic is independent of API and infrastructure concerns, making it easier to test, maintain, and extend.

## Technology Stack

| Category | Technologies |
|---|---|
| Backend | C#, .NET 10, ASP.NET Core Web API |
| Architecture | Microservices, Clean Architecture, DDD |
| Database | SQL Server, Entity Framework Core |
| Caching | Redis |
| API Gateway | YARP |
| Authentication | ASP.NET Core Identity, JWT, Refresh Tokens (Admin Only) |
| Background Processing | .NET BackgroundService |
| Validation | FluentValidation |
| Containerization | Docker, Docker Compose |
| Testing | Automated Tests |

## Key Engineering Highlights

- Designed a multi-service backend with clear service boundaries.
- Applied Clean Architecture and Domain-Driven Design.
- Implemented graph-based route optimization.
- Integrated Redis for metro network snapshot caching.
- Used background processing for network data synchronization.
- Implemented admin authentication using JWT and Refresh Tokens.
- Containerized the application and supporting infrastructure.
- Added automated tests to improve maintainability and reliability.

## Project Structure

```text
SmartMetroTripPlanner/
│
├── services/
│   ├── ApiGateway/
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
└── compose.yaml
```

---

**Smart Metro Trip Planner** demonstrates practical backend engineering with .NET, combining microservices, clean architecture, caching, background processing, and graph-based route optimization in a maintainable and extensible system.
