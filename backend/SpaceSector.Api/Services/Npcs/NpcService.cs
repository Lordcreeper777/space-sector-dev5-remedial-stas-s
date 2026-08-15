using SpaceSector.Api.Data;
using SpaceSector.Api.Dtos.Npcs;
using SpaceSector.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace SpaceSector.Api.Services.Npcs;

public class NpcService : INpcService
{
    private readonly SpaceSectorDbContext _dbContext;

    public NpcService(SpaceSectorDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Npc> CreateAsync(CreateNpcRequest request)
    {
        var cleanedName = request.Name.Trim();

        if (string.IsNullOrWhiteSpace(cleanedName))
        {
            throw new ArgumentException("NPC name cannot be empty.");
        }

        var npc = new Npc
        {
            Name = cleanedName,
            PositionX = request.PositionX,
            PositionY = request.PositionY,
            PositionZ = request.PositionZ
        };

        _dbContext.Npcs.Add(npc);
        await _dbContext.SaveChangesAsync();

        return npc;
    
    }
        public async Task<List<Npc>> GetAllAsync()
    {
        return await _dbContext.Npcs
            .AsNoTracking()
            .ToListAsync();
    }
}


