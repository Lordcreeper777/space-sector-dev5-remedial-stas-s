using SpaceSector.Api.Models;

namespace SpaceSector.Api.Services.Surveillance;

public interface ICameraVisibilityService
{
    bool CanSeeNpc(Camera camera, Npc npc);
}
