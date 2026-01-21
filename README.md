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
│  │  Dapr Output Binding         │  │
│  └──────────┬───────────────────┘  │
└─────────────┼───────────────────────┘
              │ Dapr
              ▼
┌─────────────────────────────────────┐
│         RabbitMQ Queue               │
│    (Queue: todos-create)             │
│  (Container: todo-rabbitmq)          │
└──────────┬──────────────────────────┘
           │ Message Queue
           ▼
┌─────────────────────────────────────┐
│         TodoAccessor                 │
│  ┌───────────────────────────────┐  │
│  │  Dapr Sidecar (todoaccessor)  │  │
│  └───────────┬───────────────────┘  │
│             │                        │
│  ┌──────────▼───────────────────┐  │
│  │  Dapr Input Binding          │  │
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
- **Service Communication**: 
  - Dapr service invocation for synchronous GET requests
  - RabbitMQ message queue for asynchronous POST requests
- **Containerization**: All services run in Docker containers
- **Database**: PostgreSQL with Entity Framework Core migrations
- **Message Queue**: RabbitMQ for asynchronous task processing
- **API Design**: Minimal APIs with extension methods for clean organization

## 🛠️ Technologies & Tools

### Core Technologies
- **.NET 10.0** - Latest .NET framework
- **ASP.NET Core Minimal APIs** - Lightweight API framework
- **Entity Framework Core 10.0** - ORM for database operations
- **PostgreSQL 16** - Relational database
- **Dapr 1.16** - Distributed application runtime
- **RabbitMQ 3** - Message broker for asynchronous communication

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
│   │   ├── todos-create-queue.yaml # RabbitMQ binding component
│   │   └── README.md              # Components documentation
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
│   │   ├── ITodoQueryClient.cs        # Query service interface
│   │   ├── DaprTodoQueryClient.cs     # Dapr query implementation
│   │   ├── HttpTodoQueryClient.cs     # HTTP query implementation (backup)
│   │   ├── ITodoCreatePublisher.cs    # Publisher service interface
│   │   └── DaprTodoCreatePublisher.cs  # Dapr publisher implementation
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
- **RabbitMQ AMQP**: `localhost:5672`
- **RabbitMQ Management UI**: `http://localhost:15672` (guest/guest)
- **Swagger UI (Manager)**: `http://localhost:5271/swagger`
- **Swagger UI (Accessor)**: `http://localhost:5292/swagger`

### Container Names

- `todo-manager` - TodoManager service
- `todo-accessor` - TodoAccessor service
- `todo-postgres` - PostgreSQL database
- `todo-rabbitmq` - RabbitMQ message broker
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
Creates a new todo item by publishing to RabbitMQ queue (asynchronous).

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

**Flow:**
1. TodoManager validates request
2. Generates new GUID
3. Publishes message to RabbitMQ queue via Dapr output binding
4. Returns 202 Accepted immediately
5. TodoAccessor consumes message from queue via Dapr input binding
6. TodoAccessor saves to database

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

#### 1. Service Invocation (Synchronous - GET requests)

TodoManager uses Dapr service invocation to call TodoAccessor for GET requests.

**Flow:**
1. Client calls TodoManager `GET /todos/{id}`
2. TodoManager's `DaprTodoQueryClient` uses Dapr client
3. Dapr sidecar routes request to TodoAccessor
4. TodoAccessor queries database and returns result
5. Response flows back through Dapr to TodoManager
6. TodoManager returns to client

**Code Example:**
```csharp
// In DaprTodoQueryClient.cs
var todo = await _daprClient.InvokeMethodAsync<TodoItemDto>(
    HttpMethod.Get,
    "todoaccessor",        // App ID
    $"todos/{id}");        // Endpoint path
```

#### 2. Output Binding (Asynchronous - POST requests)

TodoManager uses Dapr output binding to publish messages to RabbitMQ.

**Flow:**
1. Client calls TodoManager `POST /todos`
2. TodoManager validates and generates ID
3. `DaprTodoCreatePublisher` uses Dapr output binding
4. Dapr sidecar publishes message to RabbitMQ queue
5. TodoManager returns 202 Accepted immediately

**Code Example:**
```csharp
// In DaprTodoCreatePublisher.cs
await _daprClient.InvokeBindingAsync(
    "todos-create-queue",  // Binding name
    "create",              // Operation
    payload);              // Message payload
```

#### 3. Input Binding (Asynchronous - Queue consumption)

TodoAccessor uses Dapr input binding to consume messages from RabbitMQ.

**Flow:**
1. RabbitMQ receives message in `todos-create` queue
2. Dapr sidecar detects new message
3. Dapr calls TodoAccessor endpoint `/bindings/todos-create-queue`
4. TodoAccessor processes message and saves to database
5. Returns success to Dapr

**Component Configuration:**
```yaml
# dapr/components/todos-create-queue.yaml
apiVersion: dapr.io/v1alpha1
kind: Component
metadata:
  name: todos-create-queue
spec:
  type: bindings.rabbitmq
  metadata:
    - name: queueName
      value: todos-create
    - name: host
      value: amqp://guest:guest@rabbitmq:5672
    - name: direction
      value: "input,output"
    - name: route
      value: "/bindings/todos-create-queue"
```

### Dapr Configuration

**Sidecar Configuration:**
- **App IDs**: `todoaccessor`, `todomanager`
- **App Port**: `8080` (both services)
- **Components Path**: `/components` (mounted from `dapr/components/`)

**Dapr Components Folder:**
```
dapr/
├── components/                    # Component definitions (YAML files)
│   └── todos-create-queue.yaml   # RabbitMQ binding component
└── config.yaml                    # Dapr runtime configuration
```

## 🧪 Testing the Application

### Using Swagger UI

1. Navigate to `http://localhost:5271/swagger` (TodoManager)
2. Try the `GET /todos/{id}` endpoint
3. Try the `POST /todos` endpoint

### Using curl

```bash
# Create a todo via Manager (publishes to queue)
curl -X POST http://localhost:5271/todos \
  -H "Content-Type: application/json" \
  -d '{"title":"Test Todo","description":"Test"}'

# Get the ID from response, then get todo via Manager (uses Dapr service invocation)
curl http://localhost:5271/todos/{id-from-response}

# Get todo directly from Accessor
curl http://localhost:5292/todos/{id-from-response}

# Create todo directly via Accessor (bypasses queue)
curl -X POST http://localhost:5292/todos \
  -H "Content-Type: application/json" \
  -d '{"id":"123e4567-e89b-12d3-a456-426614174000","title":"Direct Todo","description":"Test"}'
```

### Using Visual Studio HTTP Files

Both projects include `.http` files for easy testing:
- `TodoManager/TodoManager.http`
- `TodoAccessor/TodoAccessor.http`

## 🐛 Debugging Guide - Start to Finish

### Prerequisites Check

Before debugging, ensure everything is ready:

```bash
# Check Docker is running
docker ps

# Check if ports are available
netstat -an | findstr "5271 5292 5432 5672 15672"
```

### Step 1: Start the System

#### Option A: Visual Studio (Recommended for Debugging)

1. **Open Solution**
   - Open `TodoAppLearning.sln` in Visual Studio

2. **Set Startup Project**
   - Right-click `docker-compose.dcproj` → **Set as Startup Project**

3. **Set Breakpoints** (Before starting!)
   - `TodoManager/Endpoints/TodoEndpoints.cs`:
     - Line 42: `CreateTodo` method entry
     - Line 58: `PublishAsync` call
   - `TodoManager/Services/DaprTodoCreatePublisher.cs`:
     - Line 19: `PublishAsync` method entry
   - `TodoAccessor/Endpoints/TodoEndpoints.cs`:
     - Line 78: `HandleQueueMessage` method entry
     - Line 126: Database save operation

4. **Start Debugging**
   - Press **F5** or click **Start Debugging**
   - Visual Studio will:
     - Build all projects
     - Create Docker images
     - Start all containers
     - Attach debuggers to both services

5. **Wait for Services to Start**
   - Check Output window for "Now listening on: http://[::]:8080"
   - Wait ~30 seconds for all services to be healthy

#### Option B: Command Line

```bash
# Navigate to project root
cd C:\Users\behap\source\repos\TodoAppLearning

# Build and start all services
docker compose up -d --build

# Watch logs
docker compose logs -f
```

### Step 2: Verify Services Are Running

```bash
# Check all containers are running
docker compose ps

# Expected output: All services should show "Up" status
# - todo-postgres (healthy)
# - todo-rabbitmq (healthy)
# - todo-accessor (running)
# - todo-accessor-dapr (running)
# - todo-manager (running)
# - todo-manager-dapr (running)
```

**Verify Services:**
- **PostgreSQL**: `docker exec todo-postgres pg_isready -U postgres`
- **RabbitMQ**: Open `http://localhost:15672` (login: guest/guest)
- **TodoManager**: Open `http://localhost:5271/swagger`
- **TodoAccessor**: Open `http://localhost:5292/swagger`

### Step 3: Debug GET Request Flow (Synchronous)

**Test the synchronous GET flow with Dapr service invocation:**

1. **Set Breakpoints:**
   - `TodoManager/Endpoints/TodoEndpoints.cs` line 24 (GetTodo)
   - `TodoManager/Services/DaprTodoQueryClient.cs` line 24 (GetTodoAsync)
   - `TodoAccessor/Endpoints/TodoEndpoints.cs` line 33 (GetTodo)

2. **Create a Todo First** (via Accessor directly):
   ```bash
   curl -X POST http://localhost:5292/todos \
     -H "Content-Type: application/json" \
     -d "{\"id\":\"123e4567-e89b-12d3-a456-426614174000\",\"title\":\"Test Todo\",\"description\":\"Test Description\"}"
   ```

3. **Test GET via Manager** (triggers Dapr):
   - Open Swagger: `http://localhost:5271/swagger`
   - Try `GET /todos/{id}` with the ID from step 2
   - **OR** use curl:
     ```bash
     curl http://localhost:5271/todos/123e4567-e89b-12d3-a456-426614174000
     ```

4. **Debug Flow:**
   - Breakpoint hits in `TodoManager/Endpoints/TodoEndpoints.cs`
   - Step into `queryClient.GetTodoAsync()`
   - Breakpoint hits in `DaprTodoQueryClient.cs`
   - Step through Dapr invocation
   - Breakpoint hits in `TodoAccessor/Endpoints/TodoEndpoints.cs`
   - Step through database query
   - Watch response flow back

### Step 4: Debug POST Request Flow (Asynchronous with Queue)

**Test the asynchronous POST flow with RabbitMQ queue:**

1. **Set Breakpoints:**
   - `TodoManager/Endpoints/TodoEndpoints.cs` line 40 (CreateTodo)
   - `TodoManager/Services/DaprTodoCreatePublisher.cs` line 17 (PublishAsync)
   - `TodoAccessor/Endpoints/TodoEndpoints.cs` line 78 (HandleQueueMessage)

2. **Open RabbitMQ Management UI:**
   - Navigate to `http://localhost:15672`
   - Login: `guest` / `guest`
   - Go to **Queues** tab
   - You should see `todos-create` queue (may be empty initially)

3. **Test POST via Manager:**
   - Open Swagger: `http://localhost:5271/swagger`
   - Try `POST /todos` with:
     ```json
     {
       "title": "Debug Test Todo",
       "description": "Testing queue flow"
     }
     ```
   - **OR** use curl:
     ```bash
     curl -X POST http://localhost:5271/todos \
       -H "Content-Type: application/json" \
       -d "{\"title\":\"Debug Test Todo\",\"description\":\"Testing queue flow\"}"
     ```

4. **Debug Flow - Part 1 (Manager):**
   - Breakpoint hits in `TodoManager/Endpoints/TodoEndpoints.cs` (CreateTodo)
   - Step through validation and ID generation
   - Step into `publisher.PublishAsync()`
   - Breakpoint hits in `DaprTodoCreatePublisher.cs`
   - Step through `InvokeBindingAsync` call
   - **Watch**: Message is published to queue
   - Return to `CreateTodo` - returns 202 Accepted

5. **Check RabbitMQ UI:**
   - Refresh `http://localhost:15672/queues`
   - You should see 1 message in `todos-create` queue
   - Message will be consumed within seconds

6. **Debug Flow - Part 2 (Accessor):**
   - Wait 2-5 seconds for Dapr to consume message
   - Breakpoint hits in `TodoAccessor/Endpoints/TodoEndpoints.cs` (HandleQueueMessage)
   - Step through message parsing
   - Step through database save
   - Watch message disappear from RabbitMQ UI

7. **Verify Todo Was Created:**
   ```bash
   # Get the ID from the POST response, then:
   curl http://localhost:5271/todos/{id-from-response}
   ```

### Step 5: View Logs for Troubleshooting

#### Visual Studio Output Window
- Shows application logs from both services
- Shows Dapr sidecar logs
- Shows container startup messages

#### Docker Logs

```bash
# All services
docker compose logs -f

# Specific service
docker compose logs -f todo-manager
docker compose logs -f todo-accessor

# Dapr sidecars
docker compose logs -f todo-manager-dapr
docker compose logs -f todo-accessor-dapr

# RabbitMQ
docker compose logs -f todo-rabbitmq

# Last 50 lines of specific service
docker compose logs --tail=50 todo-manager

# Follow logs with timestamps
docker compose logs -f --timestamps todo-manager
```

### Step 6: Common Debugging Scenarios

#### Scenario 1: Breakpoint Not Hitting

**Check:**
- Is debugger attached? (Check Debug → Windows → Processes)
- Is code running in container? (Check container logs)
- Is request reaching the service? (Check Swagger/curl response)

**Solution:**
- Rebuild containers: `docker compose up -d --build`
- Restart debugging in Visual Studio
- Check firewall/port conflicts

#### Scenario 2: Queue Message Not Consumed

**Check:**
- RabbitMQ UI: Is message in queue?
- Dapr logs: `docker compose logs todo-accessor-dapr`
- Accessor logs: `docker compose logs todo-accessor`

**Solution:**
- Verify Dapr component is loaded: Check Dapr logs for component errors
- Verify route matches: Component `route` should match endpoint path
- Restart Dapr sidecar: `docker restart todo-accessor-dapr`

#### Scenario 3: Dapr Service Invocation Failing

**Check:**
- Dapr logs: `docker compose logs todo-manager-dapr`
- Network connectivity: Are services on same network?
- App ID matches: Component uses correct app-id

**Solution:**
- Verify app-ids match in docker-compose and code
- Check Dapr sidecar is running: `docker ps | grep dapr`
- Verify service is listening: Check service logs

#### Scenario 4: Database Connection Issues

**Check:**
- PostgreSQL is healthy: `docker compose ps postgres`
- Connection string is correct in appsettings.json
- Migrations applied: Check Accessor logs for migration messages

**Solution:**
- Restart PostgreSQL: `docker restart todo-postgres`
- Check connection string format
- Verify network: Services must be on `todoapp-network`

### Step 7: Advanced Debugging Tips

#### Inspect Container Internals

```bash
# Enter container shell
docker exec -it todo-manager sh
docker exec -it todo-accessor sh

# Check environment variables
docker exec todo-manager env

# Check network connectivity
docker exec todo-manager ping todo-accessor
docker exec todo-accessor ping postgres
docker exec todo-accessor ping rabbitmq
```

#### Monitor RabbitMQ Queue

```bash
# Watch queue in real-time
# Open RabbitMQ UI: http://localhost:15672
# Navigate to Queues → todos-create
# Watch messages appear and disappear
```

#### Check Dapr Component Status

```bash
# Check if Dapr loaded components
docker exec todo-accessor-dapr ls -la /components

# Check Dapr logs for component errors
docker compose logs todo-accessor-dapr | grep -i component
docker compose logs todo-manager-dapr | grep -i component
```

#### Database Inspection

```bash
# Connect to PostgreSQL
docker exec -it todo-postgres psql -U postgres -d tododb

# List tables
\dt

# Query todos
SELECT * FROM "TodoItems";

# Exit
\q
```

### Step 8: Clean Restart (If Issues Persist)

```bash
# Stop all services
docker compose down

# Remove volumes (cleans database and RabbitMQ data)
docker compose down -v

# Remove images (forces rebuild)
docker compose down --rmi all

# Rebuild and start fresh
docker compose up -d --build
```

### Debugging Checklist

- [ ] All containers are running (`docker compose ps`)
- [ ] All containers are healthy (no "unhealthy" status)
- [ ] Breakpoints are set in correct files
- [ ] Debugger is attached (Visual Studio)
- [ ] Services are accessible (Swagger opens)
- [ ] RabbitMQ UI is accessible
- [ ] Database is accessible
- [ ] Dapr sidecars are running
- [ ] Network connectivity works
- [ ] Logs show no errors

### Quick Debug Commands Reference

```bash
# Status check
docker compose ps

# View logs
docker compose logs -f [service-name]

# Restart service
docker restart [container-name]

# Rebuild and restart
docker compose up -d --build [service-name]

# Clean restart
docker compose down -v && docker compose up -d --build

# Check network
docker network inspect todoapplearning_todoapp-network

# Check volumes
docker volume ls
```

## 📊 Current Status

### ✅ Completed Features

- [x] Project structure and solution setup
- [x] TodoAccessor service with Minimal APIs
- [x] PostgreSQL database integration with EF Core
- [x] Database migrations (auto-applied on startup)
- [x] TodoManager service with Minimal APIs
- [x] Docker containerization for all services
- [x] Docker Compose orchestration
- [x] Dapr sidecar integration
- [x] Dapr service invocation (GET requests)
- [x] RabbitMQ message queue integration
- [x] Dapr output binding (queue publishing)
- [x] Dapr input binding (queue consumption)
- [x] POST endpoint queue publishing (TodoManager)
- [x] Queue message consumption (TodoAccessor)
- [x] Swagger/OpenAPI documentation
- [x] Dapr components folder structure
- [x] Health checks and container dependencies
- [x] Complete debugging setup

### 🚧 In Progress / Pending

- [ ] Error handling and retry policies
- [ ] Dead letter queue configuration
- [ ] Message acknowledgment handling
- [ ] Authentication and authorization
- [ ] HTTPS/SSL configuration
- [ ] Production-ready configuration
- [ ] Unit and integration tests
- [ ] Performance monitoring and metrics

## 🔮 Next Steps

1. **Add error handling** and retry logic for queue operations
2. **Configure dead letter queue** for failed messages
3. **Add message acknowledgment** handling
4. **Implement logging and monitoring** (Application Insights, etc.)
5. **Add unit and integration tests**
6. **Add authentication and authorization**
7. **Configure HTTPS/SSL** for production
8. **Add performance monitoring** and metrics

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
- Service invocation (synchronous)
- Input/Output bindings (asynchronous)
- Component model
- Configuration management
- RabbitMQ integration

### Entity Framework Core
- Code-first migrations
- DbContext configuration
- Model constraints and validation

### ASP.NET Core Minimal APIs
- Extension methods for endpoint organization
- Dependency injection
- Request/response handling

### Message Queues
- Asynchronous message processing
- RabbitMQ integration
- Dapr bindings for queue operations
- Decoupled service communication

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

