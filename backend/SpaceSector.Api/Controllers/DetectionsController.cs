using Microsoft.AspNetCore.Mvc;
using SpaceSector.Api.Dtos.Detections;
using SpaceSector.Api.Models;
using SpaceSector.Api.Services.Detections;

namespace SpaceSector.Api.Controllers;

[ApiController]
[Route("detections")]
public class DetectionsController : ControllerBase
{
    private readonly IDetectionService _detectionService;

    public DetectionsController(IDetectionService detectionService)
    {
        _detectionService = detectionService;
    }

    [HttpPost]
public async Task<ActionResult<Detection>> Create(CreateDetectionRequest request)
{
    try
    {
        var detection = await _detectionService.CreateAsync(request);

        return StatusCode(StatusCodes.Status201Created, detection);
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