# CompanyServices API

**CompanyServices** is a fully featured backend **RESTful API** designed and developed using **.NET 8**, **MSSQL**, **Redis**, and **Docker**.  
The project focuses on delivering a modern, scalable, and high-performance employee management system that follows clean architectural principles and industry best practices.  
It provides complete CRUD functionality, integrates with an external random user generator API, and includes a fully containerized environment for consistent and reliable deployments.

## Overview

The purpose of the CompanyServices API is to offer a well-structured and developer-friendly backend environment that simplifies employee data management.  
The system is capable of creating, retrieving, updating, and deleting employee records through RESTful endpoints.  
In addition to these fundamental operations, the API includes seamless communication with an external service that supplies automatically generated random employee data.  
This feature makes it extremely easy to populate the system with test or demo data without requiring manual input.

The API also incorporates **Redis caching**, which significantly enhances overall performance by reducing the number of direct database queries.  
By caching frequently accessed data, response times are improved and server load is minimized.  
Persistent data is securely stored in a **MSSQL** relational database, ensuring data integrity, structure, and long-term reliability.

## Features

- **Complete Employee CRUD Operations**  
  The API supports creating new employees, retrieving both individual and list views, updating existing information, and deleting records.  
  Each operation is implemented in a clean and predictable RESTful manner, making it intuitive for client applications to interact with.

- **Random Employee Data Generation**  
  The system integrates with an external random user API that automatically generates realistic employee profiles.  
  This feature is helpful for testing, demo environments, automated seeding, or simply experimenting with the application without manually entering data.

- **Redis Caching for Enhanced Performance**  
  Frequently requested employee data is cached in Redis to avoid repetitive database calls.  
  This improves response times and reduces the load on the MSSQL server, resulting in a smoother application performance.

- **Persistent Storage with MSSQL**  
  Employee records are stored in a fully relational MSSQL database.  
  MSSQL provides consistency, durability, and structured storage for all employee-related information.

- **Health Check Endpoint**  
  A lightweight health check endpoint is included to help monitor service status, uptime, and application readiness.

- **Dockerized Deployment**  
  The entire application, including the API, MSSQL server, and Redis caching layer, is containerized using Docker.  
  This guarantees environment consistency and makes local development, testing, and production deployment far easier.

- **Swagger UI Integration**  
  The API comes with full Swagger and OpenAPI documentation.  
  Developers can explore available endpoints, see request/response models, and test the API in real time through an interactive UI.

## Technologies Used

- **.NET 8 / C#**  
  The core framework powering the API, offering high performance, cross-platform support, and modern development features.

- **MSSQL**  
  A relational database used for storing employee records.  
  MSSQL offers strong data consistency, robust query capabilities, and reliable long-term persistence.

- **Redis**  
  A high-speed in-memory caching solution used to improve performance by storing frequently accessed data.

- **Docker**  
  The application and its dependencies are containerized using Docker, allowing for predictable deployments and unified runtime environments.

- **Swagger / OpenAPI**  
  Provides auto-generated interactive API documentation, enabling developers to visualize and test endpoints quickly.

- **External Random User API**  
  Supplies automatically generated mock employee data, supporting testing and rapid prototyping.

## Summary

The CompanyServices API is a powerful, extensible, and easy-to-use platform designed to handle employee management with modern backend technologies.  
Its combination of Redis caching, MSSQL persistence, Docker containerization, and external API integration creates a production-ready environment that balances performance, reliability, and developer productivity.  
Whether used for real applications, training, or demonstration purposes, the system provides a solid foundation for scalable backend development.
