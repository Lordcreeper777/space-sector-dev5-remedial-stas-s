using Microsoft.AspNetCore.Mvc;
using SpaceSector.Api.Services.Surveillance;

namespace SpaceSector.Api.Controllers;

[ApiController]
[Route("surveillance")]
public class SurveillanceController : ControllerBase
{
    private readonly ISurveillanceService _surveillanceService;

    public SurveillanceController(ISurveillanceService surveillanceService)
    {
        _surveillanceService = surveillanceService;
    }

    [HttpGet("visibility")]
    public async Task<IActionResult> CanCameraSeeNpc(
        Guid cameraId,
        Guid npcId)
    {
        try
        {
            var canSee = await _surveillanceService
                .CanCameraSeeNpcAsync(cameraId, npcId);

            return Ok(new
            {
                cameraId,
                npcId,
                canSee
            });
        }
        catch (ArgumentException exception)
        {
            return BadRequest(new
            {
                error = exception.Message
            });
        }
    }
}
