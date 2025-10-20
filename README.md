# CompanyServices API
**CompanyServices** is a backend RESTful API developed using **.NET 8**, **MSSQL**, **Redis**, and **Docker**. 
It provides CRUD operations for managing employees and integrates with a random user generator API to create random employee data.
The project also includes caching for performance improvement and uses **Swagger** for easy API exploration and testing.

## Features
- Full CRUD operations (GET, POST, PULL, DELETE) for employees
- Fetch random employee data from an external API
- Redis caching for optimized performance
- MSSQL integration for persistent storage
- Health check endpoint
- Dockerized environment for easy deployment
- Swagger UI for API documentation and testing

## Technologies Used
- **.NET 8 / C#**: Core backend framework
- **MSSQL (Local)**: Relational database for data storage
- **Redis**: Used for caching responses and improving speed
- **Docker**: Containerization for consistent runtime environments
- **Swagger**: Interactive API documentation
- **External API**: Used to fetch random employee data

## Setup and Run

### 1. Clone the repository
```bash
git clone https://github.com/mavesensei/CompanyServices.git
cd CompanyServices
```
### 2. Run with docker
```
docker-compose up --build
```
### 3. Run locally (without Docker)
```
dotnet build
dotnet run
```
### 4. Access Swagger
```
localhost:5000/swagger
```
