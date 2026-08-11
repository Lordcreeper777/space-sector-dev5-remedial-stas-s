using System.ComponentModel.DataAnnotations;

namespace SpaceSector.Api.Dtos.Detections;

public class CreateDetectionRequest
{
    [Required]
    public Guid SimulationSessionId { get; set; }

    [Required]
    public Guid CameraId { get; set; }

    [Required]
    public Guid NpcId { get; set; }
}