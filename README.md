# Dev5 Project: Space Sector

Space Sector is a Unity-based neighborhood surveillance simulation with a science-fiction theme.

The player will place surveillance devices in a space colony sector and monitor distinguishable NPCs. Detected NPCs, positions, cameras, simulation sessions, blind spots, and scores will be stored in a PostgreSQL database.

## Current Status

The repository structure and initial project configuration are being prepared.

## Planned Technology

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

The project is not runnable yet. Setup instructions will be added as development progresses.

## Environment Variables

A `.env.template` file is included in the repository.

Later, create a local `.env` file based on the template:

```bash
cp .env.template .env
```

## Sources

### PostgreSQL with Docker Compose

- **Docker Compose variable interpolation:**  
  https://docs.docker.com/compose/how-tos/environment-variables/variable-interpolation/
- **PostgreSQL official Docker image:**  
  https://hub.docker.com/_/postgres
- **Docker Compose service and health-check reference:**  
  https://docs.docker.com/reference/compose-file/services/
- **Accessed:** 6 August 2026
- **Applied in:** `.env.template`, `.env`, and `docker-compose.yml`
- **Usage:** These sources helped me understand how Docker Compose reads
  environment variables, how the official PostgreSQL image is configured,
  how database data can be persisted with a named volume, and how a health
  check can determine whether PostgreSQL is ready.
- **Own implementation:** I selected the variable names, database name,
  container service name, ports, PostgreSQL version, volume name, and health
  check settings for the requirements of Space Sector.
