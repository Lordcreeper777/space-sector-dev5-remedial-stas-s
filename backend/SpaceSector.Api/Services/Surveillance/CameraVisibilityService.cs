using System.Numerics;
using SpaceSector.Api.Models;

namespace SpaceSector.Api.Services.Surveillance;

public class CameraVisibilityService : ICameraVisibilityService
{
    public bool CanSeeNpc(Camera camera, Npc npc)
    {
        var cameraPosition = new Vector2(camera.PositionX, camera.PositionZ);
        var npcPosition = new Vector2(npc.PositionX, npc.PositionZ);

        var directionToNpc = npcPosition - cameraPosition;
        var distance = directionToNpc.Length();

        if (distance > camera.Range)
        {
            return false;
        }

        if (distance == 0)
        {
            return true;
        }

        var rotationRadians = camera.RotationY * MathF.PI / 180f;

        var cameraForward = new Vector2(
            MathF.Sin(rotationRadians),
            MathF.Cos(rotationRadians));

        var normalizedDirection = Vector2.Normalize(directionToNpc);

        var dot = Vector2.Dot(cameraForward, normalizedDirection);
        dot = Math.Clamp(dot, -1f, 1f);

        var angle = MathF.Acos(dot) * 180f / MathF.PI;

        return angle <= camera.FieldOfView / 2f;
    }
}
