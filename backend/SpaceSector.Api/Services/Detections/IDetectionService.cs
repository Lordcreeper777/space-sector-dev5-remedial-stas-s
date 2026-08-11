using SpaceSector.Api.Dtos.Detections;
using SpaceSector.Api.Models;

namespace SpaceSector.Api.Services.Detections;

public interface IDetectionService
{
    Task<Detection> CreateAsync(CreateDetectionRequest request);
}