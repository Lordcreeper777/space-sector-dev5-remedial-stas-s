using Microsoft.EntityFrameworkCore;
using SpaceSector.Api.Models;

namespace SpaceSector.Api.Data;

public class SpaceSectorDbContext : DbContext
{
    public SpaceSectorDbContext(DbContextOptions<SpaceSectorDbContext> options)
        : base(options)
    {
    }

    public DbSet<Npc> Npcs => Set<Npc>();
    public DbSet<Camera> Cameras => Set<Camera>();
}