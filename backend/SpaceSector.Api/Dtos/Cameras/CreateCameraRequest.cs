using System.ComponentModel.DataAnnotations;

namespace SpaceSector.Api.Dtos.Cameras;

public class CreateCameraRequest
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    public float PositionX { get; set; }

    public float PositionY { get; set; }

    public float PositionZ { get; set; }

    public float RotationY { get; set; }

    [Range(1, 500)]
    public float Range { get; set; }

    [Range(1, 179)]
    public float FieldOfView { get; set; }
}