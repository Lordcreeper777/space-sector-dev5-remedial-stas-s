using System.ComponentModel.DataAnnotations;

namespace SpaceSector.Api.Dtos.Npcs;

public class CreateNpcRequest
{
    [Required]
    [StringLength(50, MinimumLength = 1)]
    public string Name { get; set; } = string.Empty;

    public float PositionX { get; set; }

    public float PositionY { get; set; }

    public float PositionZ { get; set; }
}