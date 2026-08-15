using SpaceSector.Api.Data;
using SpaceSector.Api.Models;

namespace SpaceSector.Api.Services.Sessions;

public class SimulationSessionService : ISimulationSessionService
{
    private readonly SpaceSectorDbContext _dbContext;

    public SimulationSessionService(SpaceSectorDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<SimulationSession> StartAsync()
    {
        var session = new SimulationSession();

        _dbContext.SimulationSessions.Add(session);
        await _dbContext.SaveChangesAsync();

        return session;
    }
}