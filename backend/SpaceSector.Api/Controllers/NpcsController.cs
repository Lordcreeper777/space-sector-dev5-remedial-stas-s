using Microsoft.AspNetCore.Mvc;
using SpaceSector.Api.Dtos.Npcs;
using SpaceSector.Api.Models;
using SpaceSector.Api.Services.Npcs;

namespace SpaceSector.Api.Controllers;

[ApiController]
[Route("npcs")]
public class NpcsController : ControllerBase
{
    private readonly INpcService _npcService;

    public NpcsController(INpcService npcService)
    {
        _npcService = npcService;
    }

    [HttpPost]
    public async Task<ActionResult<Npc>> Create(CreateNpcRequest request)
    {
        var npc = await _npcService.CreateAsync(request);

        return StatusCode(StatusCodes.Status201Created, npc);
    }
}