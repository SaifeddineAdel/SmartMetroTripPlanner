# 🚇 Smart Metro Trip Planner — Backend

A microservices-based backend built with **.NET 10 and ASP.NET Core**, responsible for metro network management, route optimization, authentication, and trip calculation.

## Overview

The backend provides the APIs and business logic that power Metro Flow.

It is responsible for:
- Managing metro network data.
- Calculating optimal routes.
- Handling metro network synchronization.
- Managing administrative authentication.
- Calculating trip duration and ticket fares.

## 🏗️ Backend Architecture

The backend follows a Microservices Architecture with three main services.

### 1. API Gateway

**Technology:** YARP Reverse Proxy

The API Gateway acts as the single entry point for frontend requests.

Responsibilities:
- Receives incoming HTTP requests.
- Routes requests to the appropriate microservice.
- Forwards trip-planning requests to TripPlanning.
- Forwards metro management requests to MetroNetwork.

### 2. MetroNetwork Service

**Technologies:** ASP.NET Core, EF Core, SQL Server, Redis

Responsible for managing the Cairo Metro network.

Main responsibilities:
- Manage metro stations.
- Manage metro lines.
- Manage line-station relationships.
- Persist network data in SQL Server.
- Publish metro network snapshots to Redis.
- Provide administrative endpoints protected by authentication.

### 3. TripPlanning Service

**Technologies:** ASP.NET Core, Redis

Responsible for finding optimal metro journeys.

Main responsibilities:
- Read the latest metro network snapshot from Redis.
- Build an in-memory graph representing the metro network.
- Find routes between stations.
- Handle transfers between metro lines.
- Calculate estimated travel time.
- Calculate ticket fares.

## 📁 Backend Structure

```text
Backend/
│
├── services/
│   │
│   ├── ApiGateway/
│   │   └── src/
│   │       └── ApiGateway.Api/
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
├── SmartMetroTripPlanner.slnx
├── compose.yaml
└── README.md
```

## 🧱 Clean Architecture

MetroNetwork and TripPlanning are organized into four main layers.

| Layer | Responsibility |
|---|---|
| API | Controllers, HTTP endpoints, and application configuration |
| Application | Use cases, DTOs, validation, and application workflows |
| Domain | Core entities and business rules |
| Infrastructure | Database access, Redis integration, and external dependencies |

### Layer Responsibilities

**API**
- Exposes HTTP endpoints.
- Handles incoming requests.
- Returns HTTP responses.
- Configures dependency injection and middleware.

**Application**
- Contains application use cases.
- Defines request and response DTOs.
- Coordinates business operations.
- Handles request validation.

**Domain**
- Contains core business entities.
- Defines domain rules and relationships.
- Remains independent of infrastructure concerns.

**Infrastructure**
- Implements database access using EF Core.
- Handles Redis communication.
- Implements infrastructure-related interfaces.

## 🔄 Backend Flows

### 1. Trip Planning Flow

```text
Frontend
   |
   v
API Gateway
   |
   v
TripPlanning Service
   |
   v
In-Memory Metro Graph
   |
   v
Route Finding
   |
   v
Trip Details Calculation
   |
   v
API Response
```

Detailed steps:

1. The API Gateway receives the trip request.
2. YARP forwards it to TripPlanning.
3. TripPlanning retrieves the relevant network graph from its in-memory state.
4. The route-finding algorithm searches for the optimal journey.
5. The service calculates station count, transfers, estimated duration, and ticket fare.
6. The response is returned to the frontend.

### 2. Metro Network Synchronization Flow

```text
SQL Server
    |
    v
MetroNetwork Service
    |
    v
BackgroundService
    |
    v
Redis Snapshot
    |
    v
TripPlanning Service
    |
    v
In-Memory Graph
```

Detailed steps:

1. MetroNetwork reads the latest metro network data from SQL Server.
2. A .NET BackgroundService publishes a snapshot to Redis.
3. TripPlanning reads the latest snapshot.
4. The snapshot is converted into an in-memory graph.
5. The graph is used to process trip requests.

This design reduces repeated database queries and unnecessary service-to-service communication during route calculation.

### 3. Administration & Authentication Flow

1. An administrator submits login credentials.
2. ASP.NET Core Identity handles authentication.
3. The backend issues JWT Access and Refresh Tokens.
4. Protected administrative endpoints validate authenticated requests.
5. Administrators can manage stations, metro lines, and line-station relationships.

Regular trip-planning functionality does not require authentication.

## 🗺️ Route Planning Engine

TripPlanning represents the metro network as a graph.

### Main Concepts

- **Stations:** Represent graph nodes.
- **Connections:** Represent movement between stations.
- **Metro Lines:** Identify the line associated with a connection.
- **Transfers:** Represent switching between metro lines.

### Route Optimization

The route selection prioritizes:

1. Minimum number of stations.
2. Minimum number of transfers.

The service also calculates:

- Ordered journey stations.
- Metro lines used.
- Transfer points.
- Estimated journey duration.
- Ticket fare.

## 💾 Data Storage & Caching

### SQL Server

Used as the persistent data store for metro network information.

### Redis

Used to store the latest metro network snapshot.

### In-Memory Graph

TripPlanning converts the Redis snapshot into a graph held in memory, allowing route calculations without querying the database for every request.

## 🛠️ Technology Stack

| Category | Technologies |
|---|---|
| Language | C# |
| Framework | .NET 10, ASP.NET Core Web API |
| Architecture | Microservices, Clean Architecture |
| Database | SQL Server |
| ORM | Entity Framework Core |
| Caching | Redis |
| API Gateway | YARP |
| Authentication | ASP.NET Core Identity, JWT, Refresh Tokens |
| Background Processing | .NET BackgroundService |
| Validation | FluentValidation |
| Containerization | Docker, Docker Compose |
| Testing | Automated Tests |

## 🧪 Engineering Practices

- SOLID principles.
- Dependency Injection.
- Dependency Inversion.
- DTO-based API contracts.
- FluentValidation.
- Centralized exception handling.
- Asynchronous programming.
- EF Core migrations.
- Unit testing.
- Separation of business logic from infrastructure.

## ⚙️ CI Pipeline

GitHub Actions runs the backend CI workflow.

The pipeline performs:

1. Repository checkout.
2. .NET 10 SDK setup.
3. NuGet dependency restoration.
4. Backend solution build.
5. Automated test execution.

The workflow is located at:

```text
.github/workflows/ci.yaml
```

---

**Smart Metro Trip Planner — A .NET microservices backend for intelligent metro journey planning.**
