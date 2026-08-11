using Microsoft.EntityFrameworkCore;
using SpaceSector.Api.Data;
using SpaceSector.Api.Dtos.Detections;
using SpaceSector.Api.Models;

namespace SpaceSector.Api.Services.Detections;

public class DetectionService : IDetectionService
{
    private readonly SpaceSectorDbContext _dbContext;

    public DetectionService(SpaceSectorDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Detection> CreateAsync(CreateDetectionRequest request)
    {
        if (request.SimulationSessionId == Guid.Empty ||
            request.CameraId == Guid.Empty ||
            request.NpcId == Guid.Empty)
        {
            throw new ArgumentException("Detection identifiers cannot be empty.");
        }

        var sessionExists = await _dbContext.SimulationSessions
            .AnyAsync(session => session.Id == request.SimulationSessionId);

        var cameraExists = await _dbContext.Cameras
            .AnyAsync(camera => camera.Id == request.CameraId);

        var npcExists = await _dbContext.Npcs
            .AnyAsync(npc => npc.Id == request.NpcId);

        if (!sessionExists || !cameraExists || !npcExists)
        {
            throw new ArgumentException(
                "The session, camera, or NPC does not exist.");
        }

        var detection = new Detection
        {
            SimulationSessionId = request.SimulationSessionId,
            CameraId = request.CameraId,
            NpcId = request.NpcId
        };

        _dbContext.Detections.Add(detection);
        await _dbContext.SaveChangesAsync();

        return detection;
    }
}