# Dev5 Project: Space Sector

Space Sector is a Unity-based neighborhood surveillance simulation with an
original science-fiction theme.

The player places surveillance devices in a space colony sector and monitors
distinguishable NPCs. NPCs, cameras, simulation sessions and detections are
persisted in PostgreSQL. The backend uses this data to calculate surveillance
coverage, blind spots and scores.

## Current Status

Space Sector is feature-complete and runs locally through Docker Compose.

Implemented:

- Unity WebGL surveillance simulation
- ASP.NET Core Web API
- PostgreSQL persistent database
- Dockerized API, database and WebGL frontend
- Persistent NPCs with unique GUIDs
- Persistent surveillance cameras
- Simulation sessions and detection records
- Camera range and field-of-view calculations
- Obstacle-based line-of-sight detection
- Blind-spot detection
- Coverage percentage and surveillance score
- Player-placeable surveillance cameras
- Persistent entity reuse to prevent duplicate NPCs and starting cameras
- Surveillance data visualization in Unity
- Browser-to-API communication through CORS
- Automatic EF Core migrations on startup
- Complete local startup using `docker compose up --build`

The complete application has been tested from a fresh PostgreSQL volume and can recreate its database schema automatically.

## Technology Stack

- Unity 2022.3 LTS
- Unity WebGL
- C#
- ASP.NET Core Web API (.NET 10)
- Entity Framework Core
- Npgsql
- PostgreSQL 17
- Docker
- Docker Compose
- Nginx

## Running the Project

The complete application runs locally using Docker Compose.

### Requirements

- Docker Desktop
- Git

Unity is only required when editing or rebuilding the Unity project. A prebuilt WebGL build is included in the repository.

### Setup

Create a local `.env` file from the template:

```bash
cp .env.template .env
```

Replace the example PostgreSQL password in `.env` with a local development password.

### Start the application

From the project root:

```bash
docker compose up --build
```

This starts:

- PostgreSQL
- ASP.NET Core Web API
- Unity WebGL application served through Nginx

Open the application at:

```text
http://localhost:8080
```

The API is available at:

```text
http://localhost:8081
```

### Stop the application

```bash
docker compose down
```

PostgreSQL data remains stored in the Docker volume.

To intentionally remove all local database data:

```bash
docker compose down -v
```

On the next startup, EF Core migrations automatically recreate the database schema.

## Environment Variables

A `.env.template` file is included in the repository.

Create a local `.env` file based on the template:

```bash
cp .env.template .env
```

Replace the example PostgreSQL password with a local development password.

The real `.env` file is ignored by Git and must not be committed.

## Database Persistence

PostgreSQL runs as the `database` service in Docker Compose.

Application data is stored in the named Docker volume:

```text
postgres_data
```

This keeps NPCs, cameras, simulation sessions and detection records available when the containers are stopped and started again.

Normal shutdown:

```bash
docker compose down
```

does not delete the stored database data.

To intentionally reset the local database:

```bash
docker compose down -v
```

The next `docker compose up --build` recreates PostgreSQL and automatically applies the EF Core migrations.

## Backend API

The backend is an ASP.NET Core Web API located in:

```text
backend/SpaceSector.Api
```

It uses controllers and services to separate responsibilities for:

- NPCs
- surveillance cameras
- simulation sessions
- detections
- surveillance calculations

Entity Framework Core with Npgsql is used to communicate with PostgreSQL.

The API automatically applies EF Core migrations when it starts.

### API address

When running through Docker Compose, the API is available at:

```text
http://localhost:8081
```

The health endpoint can be tested with:

```bash
curl http://localhost:8081/health
```

Example response:

```json
{
	"status": "healthy",
	"service": "SpaceSector.Api"
}
```

### Main API endpoints

```text
GET  /health

GET  /npcs
POST /npcs

GET  /cameras
POST /cameras

POST /sessions
POST /detections

GET  /surveillance/visibility
GET  /surveillance/summary
```

The Unity WebGL application uses these endpoints to register and reuse persistent entities, store detections and retrieve surveillance coverage data.

## Architecture

The Space Sector uses a three-part local architecture:

```text
Unity WebGL
    |
    | HTTP requests
    v
ASP.NET Core Web API
    |
    | Entity Framework Core / Npgsql
    v
PostgreSQL
```

## How to Use the Simulation

After opening `http://localhost:8080`:

- NPCs move automatically through the sector.
- Surveillance cameras scan the environment for NPCs.
- Click on the ground to place an additional surveillance camera.
- Newly placed cameras are registered through the API and stored in PostgreSQL.
- The surveillance panel shows:
  - coverage percentage
  - covered NPCs
  - blind spots
  - surveillance score
- NPC labels display their persistent backend identifier.

Camera visibility is based on range, field of view and line of sight. Buildings can block a camera's view of an NPC.

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
- **Own implementation:** I used the suggested separation as guidance and implemented
  project-specific controllers and services for NPCs, cameras, simulation sessions,
  detections and surveillance logic. I decided which abstractions were useful for
  Space Sector and tested the resulting structure throughout development.

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

**Conversation:**
<https://chatgpt.com/share/6a7f6d4a-e730-83ed-b6e8-b44f99a80e91>

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

**Usage:** AI was used to understand Unity-to-API communication, persistent entity reuse, runtime camera registration and debugging integration issues.

**Own implementation:** I decided the Unity gameplay flow, backend responsibilities and persistence approach, then implemented and tested the final integration in Space Sector.

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

### Unity WebGL and Docker deployment

- **Unity Nginx configuration for Web builds:**
  https://docs.unity3d.com/2022.3/Documentation/Manual/web-server-config-nginx.html

- **ASP.NET Core CORS:**
  https://learn.microsoft.com/aspnet/core/security/cors?view=aspnetcore-10.0

- **Official Nginx Docker image:**
  https://hub.docker.com/_/nginx

**Applied in:**

- `game/SpaceSector/nginx.conf`
- `game/SpaceSector/Dockerfile`
- `docker-compose.yml`
- `backend/SpaceSector.Api/Program.cs`
- `game/SpaceSector/WebGLBuild/`

**Usage:** Used to configure Nginx for Unity's compressed WebGL files, serve the WebGL build from Docker, and allow the browser-based Unity application on port `8080` to communicate with the ASP.NET Core API on port `8081`.

**Own implementation and testing:** Built the Unity WebGL application, added the Nginx game container, configured Docker Compose to start the game together with the API and PostgreSQL, added a restricted CORS policy for the local WebGL origin, and verified the complete application with `docker compose up --build` from a fresh PostgreSQL volume.

## Limitations

- NPC movement and camera sweep rotation are simulated live in Unity, but their transforms are not continuously synchronized to PostgreSQL.
- Surveillance coverage is calculated using the positions and rotations currently stored by the backend.
- Player-placed cameras are persisted in PostgreSQL, but previously placed runtime cameras are not recreated visually when the WebGL application is restarted.
- The project is designed for local development and demonstration through Docker Compose.

## Open-source Attribution

Space Sector uses the following open-source software and libraries:

- **ASP.NET Core** — MIT License
- **Entity Framework Core** — MIT License
- **Npgsql** — PostgreSQL License
- **PostgreSQL** — PostgreSQL License
- **NGINX Open Source** — simplified 2-clause BSD-like license

These technologies remain the property of their respective authors and contributors.  
Space Sector only contains project-specific source code and configuration built on top of these technologies.
