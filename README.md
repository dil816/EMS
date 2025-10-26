# EMS

# Event Management System

A robust, scalable, and event-driven backend system built with **.NET** for managing events, users, and communication workflows.  
This project demonstrates clean architecture, CQRS pattern, and asynchronous communication using **MassTransit**, **MediatR**, and **Docker** for containerization.

---

## Features

-  **Modular Architecture** using CQRS and MediatR
-  **Event-driven communication** with MassTransit and RabbitMQ
-  **Fully containerized** using Docker
-  **Comprehensive testing** for all major modules (Unit & Integration)
-  **Clean architecture** principles for scalability and maintainability
-  **Event Management**: Create, update, schedule, and manage events
-  **User & Role Management** (Admin, Users)

---

## Tech Stack

| Layer | Technology |
|-------|-------------|
| **Framework** | ASP.NET Core 8 Web API |
| **Architecture** | Clean Architecture + CQRS |
| **Communication** | MassTransit + RabbitMQ |
| **Mediators** | MediatR |
| **Database** | PostgreSQL |
| **Testing** | xUnit / NUnit + Moq |
| **Containerization** | Docker & Docker Compose |
| **Logging & Monitoring** | Serilog + Seq (optional) |

---

```bash
# Build and run the containers
docker-compose up --build
