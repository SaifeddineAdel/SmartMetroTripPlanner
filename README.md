# 🚇 Metro Flow — Smart Metro Trip Planner

A full-stack Cairo Metro trip-planning application built with **Angular and .NET 10 Microservices**.

Metro Flow provides an intuitive way to explore metro stations, browse metro lines, and find optimal journeys across the Cairo Metro network.

The project combines a modern Angular frontend with a scalable backend architecture built around microservices, graph-based route optimization, Redis caching, and background processing.

---

## 🎯 Project Overview

Metro Flow is designed to simplify metro journey planning.

Users can:
- Explore available metro stations.
- Browse metro lines and their stations.
- Select source and destination stations.
- Find an optimal journey.
- View transfers, estimated travel time, and ticket fares.

Administrative users can authenticate to manage metro network information.

Regular users can plan journeys without registration or login.

---

## ✨ Main Features

| Feature | Description |
|---|---|
| Trip Planning | Find optimal journeys between metro stations |
| Route Optimization | Prioritize minimum stations and transfers |
| Stations Management | Create, update, delete, and browse stations |
| Metro Lines Management | Manage metro lines and their information |
| Line Stations Management | Manage station-to-line relationships and sequence |
| Trip Details | Display stations, transfers, duration, and fare |
| Admin Authentication | Secure administrative access using JWT |
| Responsive UI | Modern Angular-based interface |

---

## 🏗️ System Architecture

The system consists of an Angular frontend and three backend services.

```text
                 USER
                  |
                  v
          ANGULAR FRONTEND
                  |
                  | HTTP Requests
                  v
             API GATEWAY
                (YARP)
                  |
          +-------+--------+
          |                |
          v                v
    METRONETWORK       TRIPPLANNING
      SERVICE             SERVICE
          |                |
          v                v
      SQL SERVER          REDIS
          |
          v
    BACKGROUND SERVICE
          |
          v
     REDIS SNAPSHOT
          |
          v
    TRIPPLANNING
          |
          v
    IN-MEMORY GRAPH
```

### System Components

**1. Frontend — Angular**
- Provides the user interface.
- Handles user interactions.
- Sends HTTP requests to the API Gateway.
- Displays stations, lines, and trip results.

**2. API Gateway — YARP**
- Acts as the single backend entry point.
- Routes requests to the appropriate service.

**3. MetroNetwork Service**
- Manages stations, metro lines, and line-station relationships.
- Stores data in SQL Server.
- Publishes metro network snapshots to Redis.

**4. TripPlanning Service**
- Maintains an in-memory graph of the metro network.
- Calculates optimal journeys.
- Calculates trip details and fares.

**5. SQL Server**
- Stores persistent metro network data.

**6. Redis**
- Stores the latest metro network snapshot.
- Provides data for TripPlanning graph construction.

---

## 🔄 End-to-End System Flow

### 1. Trip Planning Flow

This is the main user journey through the system.

```text
User
 |
 v
Angular Frontend
 |
 | Select Source & Destination
 v
API Gateway
 |
 | Route Request
 v
TripPlanning Service
 |
 | Access Current Graph
 v
In-Memory Metro Graph
 |
 | Find Optimal Route
 v
Calculate Trip Details
 |
 | Stations, Transfers, Time, Fare
 v
Return Response
 |
 v
Angular Displays Journey
```

**Step-by-step:**

1. The user selects the starting and destination stations.
2. Angular sends a trip-planning request.
3. The API Gateway forwards the request to TripPlanning.
4. TripPlanning searches its in-memory metro graph.
5. The route-finding algorithm determines the optimal journey.
6. The service calculates the journey details.
7. The result is returned through the API Gateway.
8. Angular displays the recommended journey.

### 2. Metro Network Data Flow

The metro network is managed separately from the route-planning engine.

```text
Admin
 |
 v
Angular Management Interface
 |
 v
API Gateway
 |
 v
MetroNetwork Service
 |
 v
SQL Server
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

**Why this design?**

TripPlanning does not need to query MetroNetwork or SQL Server for every trip request.

Instead, it uses a snapshot of the latest metro network data stored in Redis and maintains its own in-memory graph for efficient route calculation.

### 3. Administration Flow

1. The administrator logs in through the frontend.
2. The backend authenticates the administrator.
3. JWT Access and Refresh Tokens are used for authenticated access.
4. The administrator accesses protected management endpoints.
5. MetroNetwork processes the requested changes and persists them in SQL Server.

---

## 🗺️ Route Optimization

The TripPlanning service models the Cairo Metro network as a graph.

- Stations are represented as graph nodes.
- Connections represent possible movements between stations.
- Metro lines identify the routes used.
- Transfers represent switching between lines.

### Optimization Priorities

1. Minimum number of stations.
2. Minimum number of transfers.

### Trip Information

The result includes:
- Ordered stations.
- Metro lines used.
- Transfer points.
- Total number of stations.
- Estimated journey duration.
- Ticket fare.

---

## 💰 Ticket Fare Calculation

The project uses the following Cairo Metro fare rules:

| Number of Stations | Ticket Fare |
|---|---:|
| Up to 9 | 10 EGP |
| Up to 16 | 12 EGP |
| Up to 23 | 15 EGP |
| Up to 39 | 20 EGP |

---

## 🧱 Architecture & Engineering Principles

The backend applies Clean Architecture within its microservices.

Key principles and practices include:

- Microservices Architecture.
- Clean Architecture.
- Separation of Concerns.
- Dependency Inversion.
- SOLID principles.
- Feature-Based Organization.
- Dependency Injection.
- DTOs and Validation.
- Asynchronous Programming.
- Automated Testing.

---

## 🛠️ Technology Stack

| Category | Technologies |
|---|---|
| Frontend | Angular, TypeScript, HTML5, CSS3 |
| Backend | C#, .NET 10, ASP.NET Core Web API |
| Architecture | Microservices, Clean Architecture |
| Database | SQL Server, Entity Framework Core |
| Caching | Redis |
| API Gateway | YARP |
| Authentication | ASP.NET Core Identity, JWT, Refresh Tokens |
| Background Processing | .NET BackgroundService |
| Validation | FluentValidation |
| Containerization | Docker, Docker Compose |
| Testing | Automated Tests |
| CI/CD | GitHub Actions |

---

## 📁 Repository Structure

```text
SmartMetroTripPlanner/
│
├── Backend/
│   ├── services/
│   │   ├── ApiGateway/
│   │   ├── MetroNetwork/
│   │   │   └── src/
│   │   │       ├── MetroNetwork.Api/
│   │   │       ├── MetroNetwork.Application/
│   │   │       ├── MetroNetwork.Domain/
│   │   │       └── MetroNetwork.Infrastructure/
│   │   │
│   │   └── TripPlanning/
│   │       └── src/
│   │           ├── TripPlanning.Api/
│   │           ├── TripPlanning.Application/
│   │           ├── TripPlanning.Domain/
│   │           └── TripPlanning.Infrastructure/
│   │
│   ├── SmartMetroTripPlanner.slnx
│   ├── compose.yaml
│   └── README.md
│
├── Frontend/
│   ├── public/
│   ├── src/
│   │   └── app/
│   │       └── features/
│   │           ├── plan-trip/
│   │           ├── stations/
│   │           ├── metro-lines/
│   │           └── line-stations/
│   └── README.md
│
├── .github/
│   └── workflows/
│       └── ci.yaml
│
└── README.md
```

---

## 🧪 Continuous Integration

GitHub Actions is used to automate backend validation.

The CI workflow:
1. Checks out the repository.
2. Installs the .NET 10 SDK.
3. Restores NuGet dependencies.
4. Builds the backend solution.
5. Runs automated tests.

---

## 🚀 Project Goals

Metro Flow demonstrates practical full-stack and backend engineering through:

- Building a full-stack application.
- Designing microservices with clear responsibilities.
- Implementing graph-based route optimization.
- Using Redis for network snapshot caching.
- Synchronizing data through background processing.
- Applying Clean Architecture.
- Implementing administrative authentication.
- Containerizing services.
- Automating backend builds and tests.

---

## 📚 Documentation

For more implementation details:

- [Frontend Documentation](Frontend/README.md)
- [Backend Documentation](Backend/README.md)

---

**Metro Flow — Making Cairo Metro journeys easier.**
