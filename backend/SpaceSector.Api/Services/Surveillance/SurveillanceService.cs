using Microsoft.EntityFrameworkCore;
using SpaceSector.Api.Data;
using SpaceSector.Api.Dtos.Surveillance;

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

    public async Task<SurveillanceSummary> GetSummaryAsync()
{
    var cameras = await _dbContext.Cameras
        .AsNoTracking()
        .ToListAsync();

    var npcs = await _dbContext.Npcs
        .AsNoTracking()
        .ToListAsync();

    var blindSpotNpcIds = new List<Guid>();

    foreach (var npc in npcs)
    {
        var isCovered = cameras.Any(camera =>
            _cameraVisibilityService.CanSeeNpc(camera, npc));

        if (!isCovered)
        {
            blindSpotNpcIds.Add(npc.Id);
        }
    }

    var totalNpcs = npcs.Count;
    var blindSpotNpcs = blindSpotNpcIds.Count;
    var coveredNpcs = totalNpcs - blindSpotNpcs;

    var coveragePercentage = totalNpcs == 0
        ? 0f
        : coveredNpcs * 100f / totalNpcs;

    return new SurveillanceSummary
    {
        TotalNpcs = totalNpcs,
        CoveredNpcs = coveredNpcs,
        BlindSpotNpcs = blindSpotNpcs,
        CoveragePercentage = coveragePercentage,
        BlindSpotNpcIds = blindSpotNpcIds
    };
}
}
