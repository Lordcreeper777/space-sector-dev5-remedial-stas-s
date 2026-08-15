namespace SpaceSector.Api.Models;

public class Detection
{
    public Guid Id { get; set; } = Guid.NewGuid();

    public Guid SimulationSessionId { get; set; }

    public Guid CameraId { get; set; }

    public Guid NpcId { get; set; }

    public DateTimeOffset DetectedAtUtc { get; set; } = DateTimeOffset.UtcNow;

    public SimulationSession SimulationSession { get; set; } = null!;

    public Camera Camera { get; set; } = null!;

    public Npc Npc { get; set; } = null!;
}