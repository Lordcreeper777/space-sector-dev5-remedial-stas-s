using Microsoft.AspNetCore.Mvc;

namespace SpaceSector.Api.Controllers;

[ApiController]
[Route("health")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new
        {
            status = "healthy",
            service = "SpaceSector.Api"
        });
    }
}