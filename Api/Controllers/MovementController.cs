using Application.Dtos.Movement;
using Application.Helpers;
using Application.Interfaces;
using ClientDirectory.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovementController(IMovementHandler handler) : ControllerBase
{
    [HttpGet("account/{id}")]
    public async Task<IActionResult> GetAll(string id, [FromQuery] Filter filter)
    {
        try
        {
            var result = await handler.Get(id, filter);
            var response = Response<Paged<MovementDto>>.CreateSuccessful(result);
            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            var response = Response<string>.CreateFailed(e.Message);
            return BadRequest(response);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMovementDto dto)
    {
        try
        {
            await handler.CreateMovement(dto);
            var response = Response<string>.CreateSuccessful();
            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            var response = Response<string>.CreateFailed(e.Message);
            return BadRequest(response);
        }
    }
}