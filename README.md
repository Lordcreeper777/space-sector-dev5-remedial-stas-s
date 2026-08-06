# Space Sector

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
