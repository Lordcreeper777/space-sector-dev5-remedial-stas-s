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
- Persistent surveillance cameras with validated range and field of view
- Persistent simulation sessions and camera detection records
- Detection validation against existing NPCs, cameras, and sessions
- Camera range and field-of-view visibility calculation
- Blind-spot identification and coverage percentage
- Basic surveillance score based on coverage

  Unity connects to the local ASP.NET Core API
  Simulation sessions are created and persisted in PostgreSQL
  Unity NPCs receive persistent backend GUIDs
  Surveillance cameras receive persistent backend GUIDs
  Camera detections are sent from Unity and stored in PostgreSQL
  Existing NPCs and starting cameras are reused instead of duplicated on every Play session
  Player-placed cameras are registered and persisted through the API
  Surveillance coverage, blind spots and score are read from the backend and displayed in the Unity Game view

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

  ### NPC persistence and EF Core

- **Conversation:** https://chatgpt.com/share/6a7a2ded-267c-83eb-a122-749e5ef7ed2f
- **Questions:** Asked how NPC persistence should work with EF Core and PostgreSQL,
  why a `DbContext` and migrations are needed, and how to prove
  the data is actually persistent.
- **Applied in:** `backend/SpaceSector.Api/Data/SpaceSectorDbContext.cs`,
  `backend/SpaceSector.Api/Models/Npc.cs`,
  `backend/SpaceSector.Api/Dtos/Npcs/CreateNpcRequest.cs`,
  `backend/SpaceSector.Api/Services/Npcs/`,
  `backend/SpaceSector.Api/Controllers/NpcsController.cs`,
  and `backend/SpaceSector.Api/Migrations/`.
- **Usage:** Used the conversation to understand and review the persistence
  architecture and separation between controller, service, model, and database.
- **Own work:** Implemented the NPC persistence flow, created and applied the
  migration, tested `POST /npcs`, verified the NPC directly in PostgreSQL, and
  confirmed the same NPC remained after recreating the Docker containers.

  ### Surveillance system

- **Conversation:**  
  https://chatgpt.com/share/6a7b2bcf-6804-83ed-8300-48af62a4b2f4

- **Questions discussed:**

  - How to determine whether an NPC is inside a camera's range and field of view
  - How blind spots should work when multiple cameras exist
  - Whether an NPC should count as covered when at least one camera sees it
  - Where to validate detection IDs that do not exist
  - Whether the service should check that the session, camera and NPC exist before saving a detection
  - Troubleshooting questions related to the project

- **Applied in:**

  - `Services/Surveillance/CameraVisibilityService.cs`
  - `Services/Surveillance/SurveillanceService.cs`
  - `Services/Detections/DetectionService.cs`
  - `Controllers/DetectionsController.cs`

- **Usage:**  
  AI was used to understand and review camera visibility calculations, blind-spot logic,
  detection validation and troubleshooting decisions. The project structure, implementation
  choices and final code were applied and tested within Space Sector.

  ### AI usage – Unity API integration

**Conversation:** <https://chatgpt.com/share/6a7f6d4a-e730-83ed-b6e8-b44f99a80e91>

**Questions discussed:**

- How should Unity communicate with the ASP.NET Core API using `UnityWebRequest`?
- How can Unity serialize request data and deserialize JSON responses?
- How should persistent NPC and camera GUIDs be linked back to Unity objects?
- How can existing NPCs and cameras be reused instead of creating duplicates on every Play session?
- How should runtime-placed cameras be registered with the backend?
- How can camera detection events be sent to the API and persisted in PostgreSQL?
- How can surveillance summary data be retrieved and displayed inside Unity?
- How can Unity camera visibility use range, field of view and raycasting for obstacles?

**Applied in:**

- `SpaceSectorApiClient.cs`
- `CameraPlacement.cs`
- `NpcIdentity.cs`
- `SurveillanceCameraView.cs`
- NPC/camera GET endpoints in the ASP.NET Core backend

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

### NPC persistence with EF Core and PostgreSQL

- **EF Core migrations:**  
  https://learn.microsoft.com/ef/core/managing-schemas/migrations/
- **EF Core command-line tools:**  
  https://learn.microsoft.com/ef/core/cli/dotnet
- **ASP.NET Core model validation:**  
  https://learn.microsoft.com/aspnet/core/mvc/models/validation
- **Npgsql EF Core provider:**  
  https://www.npgsql.org/efcore/

**Applied in:** NPC model, DTO validation, `SpaceSectorDbContext`, migrations,
service layer, and PostgreSQL connection.

**Usage:** Used to understand EF Core migrations, PostgreSQL integration,
request validation, and database persistence.

**Own implementation and testing:** Created an NPC through `POST /npcs`,
verified its GUID and cleaned name in PostgreSQL, restarted the Docker stack,
and confirmed the same NPC record still existed.

### Surveillance system

- **EF Core relationships:**  
  https://learn.microsoft.com/ef/core/modeling/relationships
- **ASP.NET Core model validation:**  
  https://learn.microsoft.com/aspnet/core/mvc/models/validation
- **System.Numerics `Vector2`:**  
  https://learn.microsoft.com/dotnet/api/system.numerics.vector2
- **EF Core tracking and `AsNoTracking`:**  
  https://learn.microsoft.com/ef/core/querying/tracking

**Applied in:**

- `Models/Camera.cs`
- `Models/Detection.cs`
- `Models/SimulationSession.cs`
- `Dtos/Cameras/`
- `Dtos/Detections/`
- `Services/Surveillance/`
- `Controllers/SurveillanceController.cs`

**Usage:** Used to understand EF Core relationships, ASP.NET request validation,
read-only database queries, and vector operations used for camera range and
field-of-view calculations.

**Own implementation and testing:** Implemented camera persistence, sessions,
detections, visibility checks, blind-spot calculations, coverage percentage,
and scoring. Manually tested valid and invalid camera/detection requests and
verified stored records directly in PostgreSQL.

### Unity API integration

- **UnityWebRequest – interacting with web servers:**
  https://docs.unity3d.com/Manual/web-request.html

- **Unity JsonUtility:**
  https://docs.unity3d.com/ScriptReference/JsonUtility.html

- **Physics.Raycast:**
  https://docs.unity3d.com/6000.0/Documentation/ScriptReference/Physics.Raycast.html

- **Unity Prefabs:**
  https://docs.unity3d.com/6000.1/Documentation/Manual/Prefabs.html

- **Instantiating prefabs at runtime:**
  https://docs.unity3d.com/6000.2/Documentation/Manual/instantiating-prefabs.html

- **Object.FindObjectsByType:**
  https://docs.unity3d.com/6000.4/Documentation/ScriptReference/Object.FindObjectsByType.html

- **ASP.NET Core Web API:**
  https://learn.microsoft.com/aspnet/core/web-api/?view=aspnetcore-10.0

- **ASP.NET Core controller routing:**
  https://learn.microsoft.com/aspnet/core/mvc/controllers/routing?view=aspnetcore-10.0

**Applied in:**

- `game/SpaceSector/Assets/Scripts/SpaceSectorApiClient.cs`
- `game/SpaceSector/Assets/Scripts/CameraPlacement.cs`
- `game/SpaceSector/Assets/Scripts/NpcIdentity.cs`
- `game/SpaceSector/Assets/Scripts/SurveillanceCameraView.cs`
- `game/SpaceSector/Assets/Prefabs/Npc.prefab`
- `game/SpaceSector/Assets/Prefabs/SurveillanceCamera_01.prefab`
- `backend/SpaceSector.Api/Controllers/NpcsController.cs`
- `backend/SpaceSector.Api/Controllers/CamerasController.cs`

**Usage:** Used to understand Unity HTTP communication and JSON serialization, raycasting and line-of-sight checks, reusable prefabs and runtime prefab instantiation, locating component instances in the scene, and controller-based ASP.NET Core API endpoints.

**Own implementation and testing:** Implemented Unity API communication for sessions, NPCs, cameras and detections; reused persisted entities to prevent duplicates; registered runtime cameras; added obstacle-based camera visibility and ground-only placement; retrieved surveillance summary data; displayed persisted coverage, blind spots and score in Unity; and verified stored detections directly in PostgreSQL.
