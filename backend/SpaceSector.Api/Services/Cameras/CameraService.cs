using SpaceSector.Api.Data;
using SpaceSector.Api.Dtos.Cameras;
using SpaceSector.Api.Models;

namespace SpaceSector.Api.Services.Cameras;

public class CameraService : ICameraService
{
    private readonly SpaceSectorDbContext _dbContext;

    public CameraService(SpaceSectorDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<Camera> CreateAsync(CreateCameraRequest request)
    {
        var cleanedName = request.Name.Trim();

        if (string.IsNullOrWhiteSpace(cleanedName))
        {
            throw new ArgumentException("Camera name cannot be empty.");
        }

        var camera = new Camera
        {
            Name = cleanedName,
            PositionX = request.PositionX,
            PositionY = request.PositionY,
            PositionZ = request.PositionZ,
            RotationY = request.RotationY,
            Range = request.Range,
            FieldOfView = request.FieldOfView
        };

        _dbContext.Cameras.Add(camera);
        await _dbContext.SaveChangesAsync();

        return camera;
    }
}