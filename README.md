# Todo App

A full-stack todo application with an ASP.NET Core Web API backend, a React + Vite frontend, and SQL Server persistence.

## Features

- Create, update, delete, and list todos
- Filter by priority and category
- Search by keyword
- Mark todos as completed
- Swagger UI in development

## Tech Stack

- Backend: ASP.NET Core Web API, Entity Framework Core, AutoMapper, FluentValidation
- Frontend: React, Vite, Axios
- Database: SQL Server

## Project Structure

- TodoApi/ - ASP.NET Core Web API
- TodoApi.Tests/ - API tests
- todo-frontend/ - React frontend

## Prerequisites

- .NET 10 SDK
- Node.js 20+
- SQL Server (local or container)

## Backend Setup

1. Configure the database connection string.

- Update TodoApi/appsettings.json, or
- Set an environment variable:

```
ConnectionStrings__DefaultConnection=Server=localhost;Database=TodoDb;User Id=sa;Password=YOUR_PASSWORD;TrustServerCertificate=True
```

2. Apply EF Core migrations (once):

```
cd TodoApi
dotnet tool install --global dotnet-ef
dotnet ef database update
```

3. Run the API:

```
cd TodoApi
dotnet run
```

API URLs (development):

- http://localhost:5285
- https://localhost:7002
- Swagger UI: https://localhost:7002/swagger

## Frontend Setup

1. Configure the API base URL (optional). The default is http://localhost:5285/api/todo.

Create todo-frontend/.env:

```
VITE_API_URL=http://localhost:5285/api/todo
```

2. Install and run:

```
cd todo-frontend
npm install
npm run dev
```

## API Endpoints

Base path: /api/todo

- GET /api/todo - List all todos
- GET /api/todo/{id} - Get by id
- POST /api/todo - Create
- PUT /api/todo/{id} - Update
- DELETE /api/todo/{id} - Delete
- GET /api/todo/search?keyword=work - Search
- GET /api/todo/priority/{priority} - Filter by priority (Low, Medium, High)
- GET /api/todo/category/{category} - Filter by category (Work, Personal, Shopping, Health, Finance, Education, Entertainment, Travel, Other)

### Create/Update Payloads

Create:

```
{
  "title": "Write docs",
  "description": "Add README for project",
  "priority": "High",
  "category": "Work"
}
```

Update:

```
{
  "title": "Write docs",
  "description": "Update README and verify",
  "priority": "High",
  "category": "Work",
  "isCompleted": true
}
```

## Tests

```
dotnet test
```

## Notes

- CORS is configured to allow all origins in development.
- The frontend uses VITE_API_URL if set, otherwise it defaults to http://localhost:5285/api/todo.
