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

The PostgreSQL database is currently runnable. The Unity application and
backend API have not been created yet.

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

## AI Usage

AI assistance is documented using:

- A specific conversation link
- The question that was asked
- The file, class, function, or configuration where it was applied
- Changes made after receiving the response
- An explanation of what was learned

### Project foundation and PostgreSQL setup

- **Conversation:** Add the specific shared conversation link here.
- **Applied in:** `.gitignore`, `.env.template`, `docker-compose.yml`, and
  `README.md`
- **Usage:** AI assistance helped divide the assignment into manageable steps
  and explain the Git, Docker Compose, and PostgreSQL configuration.
- **Own work:** I created the files, selected the project-specific values,
  executed the commands, tested the database connection, and verified
  persistent storage myself.

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
  environment variables, how the official PostgreSQL image is configured, how
  database data can be persisted with a named volume, and how a health check
  can determine whether PostgreSQL is ready.
- **Own implementation:** I selected the variable names, database name,
  container service name, ports, PostgreSQL version, volume name, and health
  check settings for the requirements of Space Sector.
