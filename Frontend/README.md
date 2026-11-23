# 🚇 Metro Flow — Frontend

The frontend of **Metro Flow — Smart Metro Trip Planner** is built with Angular and TypeScript, providing a modern and responsive interface for exploring the Cairo Metro network and planning journeys.

## Overview

The frontend is responsible for:
- Providing the user interface for trip planning.
- Displaying metro stations and lines.
- Managing metro network data through administrative interfaces.
- Communicating with the backend through HTTP APIs.
- Displaying trip results and journey information.

## ✨ Features

### 🗺️ Trip Planning
- Select starting and destination stations.
- Submit trip-planning requests.
- Display the recommended route.
- View journey stations and metro lines.
- Display transfer information, estimated duration, and ticket fare.

### 🚉 Stations
- Browse available stations.
- View station information.
- Create, update, and delete stations.

### 🚆 Metro Lines
- Browse available metro lines.
- View stations associated with each line.
- Display metro line information.

### 🔗 Line Stations
- Manage relationships between metro lines and stations.
- Display station sequences within each metro line.
- Manage line-station assignments.

### 🎨 User Interface
- Responsive layout.
- Modern and clean design.
- Consistent color palette.
- Active navigation highlighting.
- Custom Metro Flow branding.

## 🏗️ Frontend Structure

The application follows a feature-based organization, grouping related components, services, and functionality together.

```text
Frontend/
│
├── public/
│   └── metro-flow.png
│
├── src/
│   │
│   ├── app/
│   │   │
│   │   ├── features/
│   │   │   ├── plan-trip/
│   │   │   │
│   │   │   ├── stations/
│   │   │   │
│   │   │   ├── metro-lines/
│   │   │   │
│   │   │   └── line-stations/
│   │   │
│   │   ├── app.ts
│   │   ├── app.html
│   │   └── app.css
│   │
│   ├── index.html
│   └── styles.css
│
└── README.md
```

## 🧩 Main Features & Responsibilities

| Feature | Responsibility |
|---|---|
| `plan-trip` | Handles station selection and displays trip results |
| `stations` | Handles station browsing and management |
| `metro-lines` | Handles metro line browsing and management |
| `line-stations` | Handles station-to-line relationships and sequence |

### Application Files

| File | Responsibility |
|---|---|
| `app.ts` | Main application component logic |
| `app.html` | Main application template |
| `app.css` | Main application styling |
| `index.html` | Main HTML entry point |
| `styles.css` | Global application styles |
| `metro-flow.png` | Application branding asset |

## 🔄 Frontend Request Flow

### Trip Planning

1. The user opens the Plan Trip page.
2. The frontend loads available stations using the Stations API.
3. The user selects the source and destination stations.
4. The frontend sends the trip request using Angular HttpClient.
5. The backend response is received through an Observable.
6. The interface displays the recommended journey and its details.

### Stations & Metro Lines Management

1. The user opens the relevant management page.
2. The feature service sends an HTTP request to the backend.
3. The backend processes the requested operation.
4. The frontend receives the response.
5. The interface updates the displayed data.

## 🔌 Communication with Backend

The frontend communicates with backend APIs using Angular HttpClient.

- **Angular Router:** Handles navigation between application pages.
- **HttpClient:** Sends HTTP requests to backend endpoints.
- **RxJS:** Handles asynchronous data streams using Observables.
- **TypeScript Interfaces:** Define the structure of API request and response data.

The frontend communicates through the API Gateway rather than directly accessing individual microservices.

## 🛠️ Technology Stack

| Technology | Purpose |
|---|---|
| Angular | Frontend framework |
| TypeScript | Application logic and type safety |
| HTML5 | Application structure |
| CSS3 | Styling and responsive layouts |
| Angular Router | Client-side navigation |
| Angular HttpClient | Backend API communication |
| RxJS | Asynchronous operations and Observables |

## 🎯 Frontend Responsibilities

The frontend focuses on presentation, user interaction, and API communication.

Business rules such as route calculation, ticket pricing, and metro network processing are handled by the backend.

---

**Metro Flow — Making Cairo Metro journeys easier.**
