using SpaceSector.Api.Dtos.Cameras;
using SpaceSector.Api.Models;

namespace SpaceSector.Api.Services.Cameras;

public interface ICameraService
{
    Task<Camera> CreateAsync(CreateCameraRequest request);
    Task<List<Camera>> GetAllAsync();
}