using SpaceSector.Api.Dtos.Npcs;
using SpaceSector.Api.Models;

namespace SpaceSector.Api.Services.Npcs;

public interface INpcService
{
    Task<Npc> CreateAsync(CreateNpcRequest request);
}
