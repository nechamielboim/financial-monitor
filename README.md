# Financial Monitor

## Project Overview

Financial Monitor is a Full Stack MVP built with ASP.NET Core and React.
The system receives financial transactions through an HTTP API, stores them in a SQLite database, and broadcasts new transactions to connected clients in real time using SignalR.

The project demonstrates clean architecture, separation of concerns, real-time communication, and responsive user experience.

---

## Technologies

### Backend

* .NET 8
* ASP.NET Core Minimal API
* Entity Framework Core
* SQLite
* SignalR

### Frontend

* React
* TypeScript
* React Router

### DevOps

* Docker 
* Kubernetes 

---

## Architecture

The system consists of two independent frontend routes:

### Transaction Simulator (`/add`)

This page simulates an external financial system by generating mock transactions and sending them to the backend using an HTTP POST request.

### Live Dashboard (`/monitor`)

This page establishes a SignalR connection with the backend and displays new transactions as they arrive in real time.

System flow:

```
Transaction Simulator
        │
        ▼
HTTP POST (/transactions)
        │
        ▼
ASP.NET Core API
        │
        ▼
SQLite Database
        │
        ▼
SignalR Hub
        │
        ▼
Live Dashboard
```

---

## API Endpoints

### POST /transactions

Receives a new transaction and stores it in the database.
After successful processing, the transaction is broadcast to connected clients using SignalR.

### GET /transactions

Returns all stored transactions.

### GET /transactions/{id}

Returns a specific transaction by its identifier.

## Thread Safety and Data Consistency

The application uses ASP.NET Core scoped lifetime management for Entity Framework Core.

Each HTTP request receives a separate DbContext instance, preventing unsafe concurrent access to the same database context.

Transaction operations are executed through Entity Framework Core, which manages database write operations using transactions. SQLite locking mechanisms help maintain data consistency during concurrent database access.

## Implemented Features

* Transaction ingestion API
* Real-time updates using SignalR
* SQLite persistent storage
* Live transaction dashboard
* Transaction simulator
* Client-side filtering 
* Status indicators (Pending, Completed, Failed)
* Animated appearance of new transactions
* Responsive UI
* Docker Compose configuration for running the application and Redis together
* Automated unit tests covering transaction processing, storage logic, and concurrency handling
  
---

## Frontend Performance

The dashboard is designed to remain responsive while receiving real-time SignalR updates.
React state management allows the UI to handle incoming transactions without blocking the browser.

---

## Testing

The project includes automated unit tests using xUnit.

The tests cover:

- Transaction creation and processing
- Correct transaction data persistence
- Retrieving transactions from storage
- Handling missing transactions
- Concurrent transaction insertion scenarios

The concurrency test validates that the system can handle multiple simultaneous transaction requests without data loss.

---

## Concurrency Handling

The application is designed to safely handle multiple concurrent client connections.

* SignalR manages multiple WebSocket connections simultaneously.
* The application uses ASP.NET Core Scoped services.
* Each HTTP request receives its own `TransactionService` and `DbContext` instance.
* Entity Framework Core with SQLite ensures consistent data persistence and avoids sharing a single `DbContext` between concurrent requests.

---

## Distributed Architecture

The project includes Redis based SignalR backplane support.

When deployed with multiple backend replicas, Redis Pub/Sub allows all instances to broadcast transaction events to all connected clients.

Architecture:

Backend Pod A
      |
      |
   Redis Backplane
      |
      |
Backend Pod B

---

## Running the Project

### Backend

```bash
cd FinancialMonitor.API
dotnet restore
dotnet run
```

### Frontend

```bash
cd financial-monitor-client
npm install
npm run dev
```

## Running with Docker Compose

The entire application can also be started using Docker Compose.

```bash
docker compose up --build
```

This starts:

- Financial Monitor API
- Redis
- Persistent SQLite volume

The frontend runs on:

```
http://localhost:5173
```

The backend runs on:

```
https://localhost:5177
```

---

## Docker

The backend uses a multi-stage Docker build:
- SDK image for compilation
- ASP.NET runtime image for production execution

This reduces the final image size and improves deployment efficiency.

## Kubernetes Deployment

The project includes Kubernetes manifests:

`deployment.yaml`
- Defines backend replicas and container configuration.

`service.yaml`
- Exposes the backend service inside the cluster.
  
## Project Structure

```
FinancialMonitor
│
├── FinancialMonitor.API
│   ├── APIs
│   ├── Data
│   ├── DTOs
│   ├── Hubs
│   ├── Interfaces
│   ├── Messaging
│   ├── Models
│   └── Services

├── FinancialMonitor.Tests
│   └── TransactionServiceTests.cs

├── financial-monitor-client
│   ├── pages
│   ├── services
│   ├── types
│
├── Dockerfile
├── docker-compose.yml
├── k8s
│   ├── deployment.yaml
│   └── service.yaml
└── README.md
```

