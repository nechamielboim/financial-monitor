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

---

## Concurrency Handling

The application is designed to safely handle multiple concurrent client connections.

* SignalR manages multiple WebSocket connections simultaneously.
* The application uses ASP.NET Core Scoped services.
* Each HTTP request receives its own `TransactionService` and `DbContext` instance.
* Entity Framework Core with SQLite ensures consistent data persistence and avoids sharing a single `DbContext` between concurrent requests.

---

## Distributed Architecture

When running multiple backend instances (Pods), clients connected to different instances would not automatically receive the same SignalR events.

A production-ready solution is to use **Redis SignalR** (or Redis Pub/Sub), allowing all application instances to synchronize real-time messages across the cluster.

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

The frontend runs on:

```
http://localhost:5173
```

The backend runs on:

```
https://localhost:7230
```

---

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
│
├── financial-monitor-client
│   ├── pages
│   ├── services
│   ├── types
│   └── components
│
├── Dockerfile
├── deployment.yaml
├── service.yaml
└── README.md
```

