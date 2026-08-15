using Microsoft.AspNetCore.Mvc;
using SpaceSector.Api.Models;
using SpaceSector.Api.Services.Sessions;

namespace SpaceSector.Api.Controllers;

[ApiController]
[Route("sessions")]
public class SimulationSessionsController : ControllerBase
{
    private readonly ISimulationSessionService _sessionService;

    public SimulationSessionsController(ISimulationSessionService sessionService)
    {
        _sessionService = sessionService;
    }

    [HttpPost]
    public async Task<ActionResult<SimulationSession>> Start()
    {
        var session = await _sessionService.StartAsync();

        return StatusCode(StatusCodes.Status201Created, session);
    }
}