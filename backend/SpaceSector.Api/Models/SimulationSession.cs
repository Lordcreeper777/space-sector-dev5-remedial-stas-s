namespace SpaceSector.Api.Models;

public class SimulationSession
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public DateTimeOffset StartedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public DateTimeOffset? EndedAtUtc { get; set; }
}