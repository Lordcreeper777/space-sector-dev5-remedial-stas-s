using Microsoft.EntityFrameworkCore;
using SpaceSector.Api.Data;

namespace SpaceSector.Api.Services.Surveillance;

public class SurveillanceService : ISurveillanceService
{
    private readonly SpaceSectorDbContext _dbContext;
    private readonly ICameraVisibilityService _cameraVisibilityService;

    public SurveillanceService(
        SpaceSectorDbContext dbContext,
        ICameraVisibilityService cameraVisibilityService)
    {
        _dbContext = dbContext;
        _cameraVisibilityService = cameraVisibilityService;
    }

    public async Task<bool> CanCameraSeeNpcAsync(Guid cameraId, Guid npcId)
    {
        var camera = await _dbContext.Cameras
            .FirstOrDefaultAsync(camera => camera.Id == cameraId);

        var npc = await _dbContext.Npcs
            .FirstOrDefaultAsync(npc => npc.Id == npcId);

        if (camera is null || npc is null)
        {
            throw new ArgumentException("Camera or NPC does not exist.");
        }

        return _cameraVisibilityService.CanSeeNpc(camera, npc);
    }
}
