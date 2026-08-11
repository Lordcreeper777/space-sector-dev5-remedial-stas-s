using Microsoft.AspNetCore.Mvc;
using SpaceSector.Api.Dtos.Cameras;
using SpaceSector.Api.Models;
using SpaceSector.Api.Services.Cameras;

namespace SpaceSector.Api.Controllers;

[ApiController]
[Route("cameras")]
public class CamerasController : ControllerBase
{
    private readonly ICameraService _cameraService;

    public CamerasController(ICameraService cameraService)
    {
        _cameraService = cameraService;
    }

    [HttpPost]
    public async Task<ActionResult<Camera>> Create(CreateCameraRequest request)
    {
        var camera = await _cameraService.CreateAsync(request);

        return StatusCode(StatusCodes.Status201Created, camera);
    }
}