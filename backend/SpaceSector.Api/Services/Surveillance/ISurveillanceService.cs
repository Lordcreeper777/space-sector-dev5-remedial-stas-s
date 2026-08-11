namespace SpaceSector.Api.Services.Surveillance;

public interface ISurveillanceService
{
    Task<bool> CanCameraSeeNpcAsync(Guid cameraId, Guid npcId);
}
