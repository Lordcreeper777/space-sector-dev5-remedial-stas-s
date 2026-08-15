using SpaceSector.Api.Models;

namespace SpaceSector.Api.Services.Sessions;

public interface ISimulationSessionService
{
    Task<SimulationSession> StartAsync();
}