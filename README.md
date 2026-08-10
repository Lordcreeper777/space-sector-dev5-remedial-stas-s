# Dev5 Project: Space Sector

Space Sector is a Unity-based neighborhood surveillance simulation with an
original science-fiction theme.

The player will place surveillance devices in a space colony sector and monitor
distinguishable NPCs. Detected NPCs, positions, cameras, simulation sessions,
blind spots, and scores will be stored in a PostgreSQL database.

## Current Status

Completed:

- Project repository and branch structure
- Unity-focused `.gitignore`
- Environment variable template
- PostgreSQL running through Docker Compose
- PostgreSQL health check
- Persistent database storage verification
- ASP.NET Core backend project created
- Controller-based API routing configured
- Project-specific `/health` endpoint created and tested
  ASP.NET Core API containerized with Docker
- Multi-stage Docker build configured
- API and PostgreSQL started together with Docker Compose
- API waits for PostgreSQL health check before starting
- Dockerized `/health` endpoint tested through `localhost:8081`
- `docker compose up --build` successfully tested

The PostgreSQL database and basic ASP.NET Core API are currently runnable.
The Unity application and database-backed API features have not been created yet.

## Technology Stack

- Unity
- C#
- ASP.NET Core Web API
- PostgreSQL
- Docker
- Docker Compose

## Planned Features

- NPCs with persistent unique identifiers
- NPC movement through a space colony sector
- Placeable surveillance cameras
- Camera field-of-view detection
- Persistent detection records
- NPC position history
- Blind-spot calculations
- Score calculations
- Data visualization in Unity
- Local startup using Docker Compose

## Running the Project

The complete application is not runnable yet.

The PostgreSQL database can already be started locally using Docker Compose.
Instructions are provided in the [Running PostgreSQL](#running-postgresql)
section.

## Environment Variables

A `.env.template` file is included in the repository.

Create a local `.env` file based on the template:

```bash
cp .env.template .env
```

Replace the example PostgreSQL password with a local development password.

The real `.env` file is ignored by Git and must not be committed.

## Running PostgreSQL

Docker Desktop must be installed and running.

Start PostgreSQL:

```bash
docker compose up -d
```

Check its status:

```bash
docker compose ps
```

The database service should display a status containing `(healthy)`.

Stop PostgreSQL:

```bash
docker compose down
```

Database data is stored in a persistent Docker volume. Do not use:

```bash
docker compose down -v
```

unless the stored database data should be deleted.

## Backend API

The backend uses ASP.NET Core Web API with C# and is located in:

```text
backend/SpaceSector.Api
```

A controller-based structure is used to keep future features such as NPCs,
cameras, detections, and sessions separated.

The current API contains a health endpoint:

```http
GET /health
```

This endpoint was manually tested and returns a healthy status when the API is running.

### Running the Backend API

From the project root:

```bash
cd backend/SpaceSector.Api
dotnet restore
dotnet run
```

The API can be checked using:

```bash
curl http://localhost:5135/health
```

A successful response is:

```json
{
	"status": "healthy",
	"service": "SpaceSector.Api"
}
```

## AI Usage

### ASP.NET Core backend structure

- **Conversation:**  
  https://chatgpt.com/share/6a75ad45-cfb0-83eb-9a90-92136ed818c0
- **Question:** How should I structure my ASP.NET Core backend for NPCs,
  cameras, detections, and sessions while following SOLID principles?
- **Follow-up:** Asked what should remain in `Program.cs` and what should be
  moved into controllers or services as the project grows.
- **Applied in:** Planning the structure of `backend/SpaceSector.Api`
- **Usage:** The conversation helped me understand the separation of
  responsibilities between `Program.cs`, controllers, services, repositories,
  and models.
- **Own implementation:** I will introduce these abstractions gradually when
  the related features are implemented instead of generating the entire
  backend at once.

### Project foundation and PostgreSQL setup

- **Conversation:**  
  https://chatgpt.com/share/6a75b354-3e60-83eb-873b-38f351bde4ec
- **Question:** How should I set up PostgreSQL with Docker Compose using a
  `.env` file and persistent storage?
- **Follow-up:** Asked for a simpler explanation of the difference between
  `docker compose restart`, `docker compose down`, and
  `docker compose down -v`.
- **Applied in:** `.env.template`, `docker-compose.yml`, and the PostgreSQL
  persistence testing process.
- **Usage:** The conversation helped me understand how Docker volumes keep
  PostgreSQL data separate from the container and why removing a container
  does not normally remove the database.
- **Own work:** I configured the project-specific PostgreSQL service and
  manually tested persistence by inserting data, restarting the container,
  and checking that the data was still available.

  ### Dockerized API and Docker troubleshooting

- **Conversation:** https://chatgpt.com/share/6a79b99a-d698-83eb-92f1-bf3014870afc
- **Questions:** Asked about the purpose of `.dockerignore`, Docker port mapping,
  container port conflicts, and how to diagnose a missing `Dockerfile` error.
- **Applied in:** `backend/SpaceSector.Api/Dockerfile`,
  `backend/SpaceSector.Api/.dockerignore`, and `docker-compose.yml`.
- **Usage:** Used the conversation to better understand the Docker setup and
  review the choices made for the API container.
- **Own work:** Moved the Docker files to the correct API directory, rebuilt
  the containers, and manually verified the `/health` endpoint through
  `localhost:8081`.

## Sources

### PostgreSQL with Docker Compose

- **Docker Compose variable interpolation:**  
  https://docs.docker.com/compose/how-tos/environment-variables/variable-interpolation/
- **PostgreSQL official Docker image:**  
  https://hub.docker.com/_/postgres
- **Docker Compose service and health-check reference:**  
  https://docs.docker.com/reference/compose-file/services/
- **Applied in:** `.env.template`, `.env`, and `docker-compose.yml`
- **Usage:** These sources helped me understand how Docker Compose reads
  environment variables, how the official PostgreSQL image is configured, how
  database data can be persisted with a named volume, and how a health check
  can determine whether PostgreSQL is ready.
- **Own implementation:** I selected the variable names, database name,
  container service name, ports, PostgreSQL version, volume name, and health
  check settings for the requirements of Space Sector.

### ASP.NET Core Web API

- **Source:**  
  https://learn.microsoft.com/aspnet/core/web-api/
- **Applied in:** `backend/SpaceSector.Api/Controllers/HealthController.cs`
  and `Program.cs`
- **Usage:** Used to understand controller-based ASP.NET Core Web APIs,
  controller routing, `ControllerBase`, and HTTP actions.
- **Own implementation:** I removed the generated WeatherForecast example and
  created a project-specific `/health` endpoint for Space Sector.

### .NET Web API project template

- **Source:**  
  https://learn.microsoft.com/dotnet/core/tools/dotnet-new-sdk-templates
- **Accessed:** 7 August 2026
- **Applied in:** `backend/SpaceSector.Api`
- **Usage:** Used to understand the `dotnet new webapi` template and the
  `--use-controllers` option.
- **Own implementation:** I selected the project name and folder structure,
  used the controller-based template, removed unnecessary generated example
  code, and tested the API locally.

### Microsoft.OpenApi package

- **Source:**  
  https://www.nuget.org/packages/Microsoft.OpenApi/2.11.0
- **Applied in:** `backend/SpaceSector.Api/SpaceSector.Api.csproj`
- **Usage:** The generated project initially contained an older package version
  that produced a vulnerability warning. I updated it to version `2.11.0` and
  confirmed that the project restored and built successfully.

  ### Dockerized ASP.NET Core API

- **ASP.NET Core with Docker:**  
  https://learn.microsoft.com/aspnet/core/host-and-deploy/docker/building-net-docker-images?view=aspnetcore-10.0
- **Docker multi-stage builds:**  
  https://docs.docker.com/build/building/multi-stage/
- **Docker build context and `.dockerignore`:**  
  https://docs.docker.com/build/concepts/context/
- **Docker Compose startup order and health checks:**  
  https://docs.docker.com/compose/how-tos/startup-order/
- **Docker Compose `up`:**  
  https://docs.docker.com/reference/cli/docker/compose/up/
- **ASP.NET Core container port 8080:**  
  https://learn.microsoft.com/dotnet/core/compatibility/containers/8.0/aspnet-port

**Applied in:**

- `backend/SpaceSector.Api/Dockerfile`
- `backend/SpaceSector.Api/.dockerignore`
- `docker-compose.yml`

**Usage:** The documentation was used to understand multi-stage .NET container builds,
Docker build contexts, service dependency health checks, port mapping, and running
the local services with `docker compose up --build`.

**Own implementation and testing:** Configured the Dockerfile and Compose API
service for Space Sector, built the containers locally, verified PostgreSQL became
healthy, and manually tested `GET /health` through `http://localhost:8081`.
