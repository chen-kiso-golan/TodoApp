# 📝 TodoApp Learning

A modern, distributed Todo application built with .NET 10, demonstrating microservices architecture, Docker containerization, and Dapr integration.

## 🎯 Project Overview

This project is a learning exercise to understand microservices architecture, containerization, and distributed application patterns. It consists of two microservices that work together to manage todo items:

- **TodoManager**: The API gateway that handles client requests and orchestrates business logic
- **TodoAccessor**: The data access service that manages database operations

## 🏗️ Architecture

```
┌─────────────────┐
│   Client/API    │
│   Consumer      │
└────────┬────────┘
         │ HTTP
         ▼
┌─────────────────────────────────────┐
│         TodoManager                  │
│  ┌───────────────────────────────┐  │
│  │   Dapr Sidecar (todomanager)  │  │
│  └───────────┬───────────────────┘  │
│             │                        │
│  ┌──────────▼───────────────────┐  │
│  │  Dapr Service Invocation     │  │
│  └──────────┬───────────────────┘  │
└─────────────┼───────────────────────┘
              │ Dapr
              ▼
┌─────────────────────────────────────┐
│         TodoAccessor                 │
│  ┌───────────────────────────────┐  │
│  │  Dapr Sidecar (todoaccessor)  │  │
│  └───────────┬───────────────────┘  │
│             │                        │
│  ┌──────────▼───────────────────┐  │
│  │  Entity Framework Core       │  │
│  └──────────┬───────────────────┘  │
└─────────────┼───────────────────────┘
              │
              ▼
┌─────────────────────────────────────┐
│      PostgreSQL Database             │
│      (Container: todo-postgres)      │
└─────────────────────────────────────┘
```

### Architecture Principles

- **Separation of Concerns**: Manager handles business logic, Accessor handles data persistence
- **Service Communication**: Dapr service invocation for inter-service communication
- **Containerization**: All services run in Docker containers
- **Database**: PostgreSQL with Entity Framework Core migrations
- **API Design**: Minimal APIs with extension methods for clean organization

## 🛠️ Technologies & Tools

### Core Technologies
- **.NET 10.0** - Latest .NET framework
- **ASP.NET Core Minimal APIs** - Lightweight API framework
- **Entity Framework Core 10.0** - ORM for database operations
- **PostgreSQL 16** - Relational database
- **Dapr 1.16** - Distributed application runtime

### Development Tools
- **Docker & Docker Compose** - Containerization and orchestration
- **Swagger/OpenAPI** - API documentation and testing
- **Visual Studio** - IDE with Docker Compose debugging support

### NuGet Packages

#### TodoAccessor
- `Microsoft.EntityFrameworkCore` (10.0.1)
- `Microsoft.EntityFrameworkCore.Design` (10.0.1)
- `Microsoft.EntityFrameworkCore.Tools` (10.0.1)
- `Npgsql.EntityFrameworkCore.PostgreSQL` (10.0.0)
- `Swashbuckle.AspNetCore` (10.1.0)

#### TodoManager
- `Dapr.AspNetCore` (1.16.1)
- `Swashbuckle.AspNetCore` (10.1.0)

## 📁 Project Structure

```
TodoAppLearning/
├── dapr/                          # Dapr configuration folder
│   ├── components/                # Dapr component definitions
│   │   └── (RabbitMQ components will be added here)
│   └── config.yaml                # Dapr runtime configuration
│
├── TodoAccessor/                  # Data access microservice
│   ├── Data/
│   │   └── TodoDbContext.cs      # EF Core DbContext
│   ├── Endpoints/
│   │   └── TodoEndpoints.cs      # Minimal API endpoints
│   ├── Migrations/                # EF Core database migrations
│   ├── Models/
│   │   └── TodoItem.cs           # Domain model
│   ├── Program.cs                 # Application entry point
│   ├── appsettings.json          # Configuration
│   └── Dockerfile                 # Container definition
│
├── TodoManager/                   # Business logic microservice
│   ├── Contracts/
│   │   ├── CreateTodoRequest.cs  # Request DTO
│   │   └── TodoItemDto.cs        # Response DTO
│   ├── Endpoints/
│   │   └── TodoEndpoints.cs      # Minimal API endpoints
│   ├── Services/
│   │   ├── ITodoQueryClient.cs   # Service interface
│   │   ├── DaprTodoQueryClient.cs # Dapr implementation
│   │   └── HttpTodoQueryClient.cs # HTTP implementation (backup)
│   ├── Program.cs                 # Application entry point
│   ├── appsettings.json          # Configuration
│   └── Dockerfile                 # Container definition
│
├── docker-compose.yml             # Main Docker Compose file
├── docker-compose.dapr.yaml       # Dapr-specific overrides
└── README.md                      # This file
```

## 🚀 Getting Started

### Prerequisites

- **.NET 10.0 SDK** - [Download](https://dotnet.microsoft.com/download)
- **Docker Desktop** - [Download](https://www.docker.com/products/docker-desktop)
- **Visual Studio 2022** (recommended) or VS Code
- **PostgreSQL Client** (optional, for direct database access)

### Initial Setup

1. **Clone the repository** (if applicable)
   ```bash
   git clone <repository-url>
   cd TodoAppLearning
   ```

2. **Restore NuGet packages**
   ```bash
   dotnet restore
   ```

3. **Build the solution**
   ```bash
   dotnet build
   ```

## 🐳 Running with Docker Compose

### Quick Start

```bash
# Build and start all services
docker compose up -d --build

# View logs
docker compose logs -f

# Stop all services
docker compose down

# Stop and remove volumes (cleans database)
docker compose down -v
```

### Service Ports

- **TodoManager**: `http://localhost:5271`
- **TodoAccessor**: `http://localhost:5292`
- **PostgreSQL**: `localhost:5432`
- **Swagger UI (Manager)**: `http://localhost:5271/swagger`
- **Swagger UI (Accessor)**: `http://localhost:5292/swagger`

### Container Names

- `todo-manager` - TodoManager service
- `todo-accessor` - TodoAccessor service
- `todo-postgres` - PostgreSQL database
- `todo-manager-dapr` - Dapr sidecar for TodoManager
- `todo-accessor-dapr` - Dapr sidecar for TodoAccessor

## 🔧 Development Workflow

### Running in Visual Studio

1. **Set Docker Compose as startup project**
   - Right-click `docker-compose.dcproj` → Set as Startup Project

2. **Start Debugging (F5)**
   - Visual Studio will:
     - Build all projects
     - Create Docker images
     - Start all containers
     - Attach debuggers to both services

3. **Set Breakpoints**
   - You can debug both services simultaneously
   - Breakpoints work in both `TodoManager` and `TodoAccessor`

### Running Locally (without Docker)

#### TodoAccessor
```bash
cd TodoAccessor
dotnet run
# Runs on http://localhost:5000 (or port in launchSettings.json)
```

#### TodoManager
```bash
cd TodoManager
dotnet run
# Runs on http://localhost:5001 (or port in launchSettings.json)
```

**Note**: For local development, you'll need:
- PostgreSQL running locally
- Update connection strings in `appsettings.json`
- Dapr CLI installed and running (for Dapr features)

## 📡 API Endpoints

### TodoManager Service

#### GET `/todos/{id}`
Retrieves a todo item by ID.

**Request:**
```http
GET http://localhost:5271/todos/123e4567-e89b-12d3-a456-426614174000
```

**Response (200 OK):**
```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "title": "Learn Dapr",
  "description": "Study Dapr service invocation",
  "createdAtUtc": "2024-01-11T13:31:02Z"
}
```

**Response (404 Not Found):**
```json
(empty body)
```

#### POST `/todos`
Creates a new todo item (currently returns accepted, queue integration pending).

**Request:**
```http
POST http://localhost:5271/todos
Content-Type: application/json

{
  "title": "New Todo",
  "description": "Todo description"
}
```

**Response (202 Accepted):**
```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000"
}
```

**Response (400 Bad Request):**
```json
{
  "error": "Title is required and cannot be empty or whitespace"
}
```

### TodoAccessor Service

#### GET `/todos/{id}`
Retrieves a todo item directly from the database.

**Request:**
```http
GET http://localhost:5292/todos/123e4567-e89b-12d3-a456-426614174000
```

**Response (200 OK):**
```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "title": "Learn Dapr",
  "description": "Study Dapr service invocation",
  "createdAtUtc": "2024-01-11T13:31:02Z"
}
```

#### POST `/todos`
Creates a new todo item in the database.

**Request:**
```http
POST http://localhost:5292/todos
Content-Type: application/json

{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "title": "New Todo",
  "description": "Todo description"
}
```

**Response (201 Created):**
```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "title": "New Todo",
  "description": "Todo description",
  "createdAtUtc": "2024-01-11T13:31:02Z"
}
```

**Response (409 Conflict):**
```json
{
  "message": "Todo with id 123e4567-e89b-12d3-a456-426614174000 already exists"
}
```

## 🗄️ Database

### PostgreSQL Configuration

- **Database**: `tododb`
- **Username**: `postgres`
- **Password**: `postgres`
- **Port**: `5432`
- **Host**: `postgres` (in Docker) or `localhost` (local)

### Entity Framework Migrations

Migrations are automatically applied on application startup. To create a new migration:

```bash
cd TodoAccessor
dotnet ef migrations add MigrationName
```

To apply migrations manually:

```bash
dotnet ef database update
```

### Database Schema

**TodoItems Table:**
- `Id` (Guid, Primary Key)
- `Title` (string, Required, MaxLength: 200)
- `Description` (string, Nullable, MaxLength: 2000)
- `CreatedAtUtc` (DateTime, Required)

## 🔄 Dapr Integration

### Current Implementation

**Service Invocation**: TodoManager uses Dapr to invoke TodoAccessor services.

**Flow:**
1. Client calls TodoManager `/todos/{id}`
2. TodoManager's `DaprTodoQueryClient` uses Dapr client
3. Dapr sidecar routes request to TodoAccessor
4. TodoAccessor queries database and returns result
5. Response flows back through Dapr to TodoManager
6. TodoManager returns to client

### Dapr Configuration

**Sidecar Configuration:**
- **App IDs**: `todoaccessor`, `todomanager`
- **App Port**: `8080` (both services)
- **Components Path**: `/components` (mounted from `dapr/components/`)

**Dapr Components Folder:**
```
dapr/
├── components/          # Component definitions (YAML files)
└── config.yaml         # Dapr runtime configuration
```

### Dapr Service Invocation Example

```csharp
// In DaprTodoQueryClient.cs
var todo = await _daprClient.InvokeMethodAsync<TodoItemDto>(
    HttpMethod.Get,
    "todoaccessor",        // App ID
    $"todos/{id}");        // Endpoint path
```

## 🧪 Testing the Application

### Using Swagger UI

1. Navigate to `http://localhost:5271/swagger` (TodoManager)
2. Try the `GET /todos/{id}` endpoint
3. Try the `POST /todos` endpoint

### Using curl

```bash
# Create a todo via Accessor
curl -X POST http://localhost:5292/todos \
  -H "Content-Type: application/json" \
  -d '{"id":"123e4567-e89b-12d3-a456-426614174000","title":"Test Todo","description":"Test"}'

# Get todo via Manager (uses Dapr)
curl http://localhost:5271/todos/123e4567-e89b-12d3-a456-426614174000

# Get todo directly from Accessor
curl http://localhost:5292/todos/123e4567-e89b-12d3-a456-426614174000
```

### Using Visual Studio HTTP Files

Both projects include `.http` files for easy testing:
- `TodoManager/TodoManager.http`
- `TodoAccessor/TodoAccessor.http`

## 🐛 Debugging

### Debugging in Visual Studio

1. **Set breakpoints** in endpoint handlers
2. **Start debugging** (F5) with Docker Compose
3. **Make API calls** via Swagger or HTTP files
4. **Step through code** as requests flow through services

### Viewing Logs

```bash
# All services
docker compose logs -f

# Specific service
docker compose logs -f todo-manager
docker compose logs -f todo-accessor
docker compose logs -f todo-accessor-dapr
docker compose logs -f todo-manager-dapr

# Last 50 lines
docker compose logs --tail=50 todo-manager
```

### Checking Container Status

```bash
# List all containers
docker compose ps

# Check container health
docker ps

# Inspect container
docker inspect todo-manager
```

## 📊 Current Status

### ✅ Completed Features

- [x] Project structure and solution setup
- [x] TodoAccessor service with Minimal APIs
- [x] PostgreSQL database integration with EF Core
- [x] Database migrations (auto-applied on startup)
- [x] TodoManager service with Minimal APIs
- [x] Docker containerization for both services
- [x] Docker Compose orchestration
- [x] Dapr sidecar integration
- [x] Dapr service invocation (GET requests)
- [x] Swagger/OpenAPI documentation
- [x] Dapr components folder structure
- [x] Health checks and container dependencies

### 🚧 In Progress / Pending

- [ ] RabbitMQ message queue integration
- [ ] Dapr input/output bindings for queue
- [ ] POST endpoint queue publishing (TodoManager)
- [ ] Queue message consumption (TodoAccessor)
- [ ] Error handling and retry policies
- [ ] Authentication and authorization
- [ ] HTTPS/SSL configuration
- [ ] Production-ready configuration

## 🔮 Next Steps

1. **Add RabbitMQ** to Docker Compose
2. **Create Dapr binding component** (`dapr/components/todos-create-queue.yaml`)
3. **Implement queue publishing** in TodoManager POST endpoint
4. **Implement queue consumption** in TodoAccessor
5. **Add error handling** and retry logic
6. **Add logging and monitoring**
7. **Add unit and integration tests**

## 📚 Key Concepts Learned

### Microservices Architecture
- Service separation (Manager vs Accessor)
- Inter-service communication patterns
- API gateway pattern

### Docker & Containerization
- Multi-stage Docker builds
- Docker Compose orchestration
- Container networking
- Volume management

### Dapr
- Sidecar pattern
- Service invocation
- Component model
- Configuration management

### Entity Framework Core
- Code-first migrations
- DbContext configuration
- Model constraints and validation

### ASP.NET Core Minimal APIs
- Extension methods for endpoint organization
- Dependency injection
- Request/response handling

## 🤝 Contributing

This is a learning project. Feel free to:
- Experiment with different patterns
- Add new features
- Improve documentation
- Share learnings

## 📝 License

This project is for educational purposes.

## 🙏 Acknowledgments

Built as a learning exercise to understand:
- Microservices architecture
- Containerization with Docker
- Distributed application patterns with Dapr
- Modern .NET development practices

---

**Happy Learning! 🚀**

