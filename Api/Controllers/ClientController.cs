using Application.Dtos.Client;
using Application.Helpers;
using Application.Interfaces;
using ClientDirectory.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController(IClientHandler handler) : ControllerBase
{
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Filter filter)
    {
        try
        {
            var result = await handler.Get(filter);
            var response = Response<Paged<ClientDto>>.CreateSuccessful(result);
            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            var response = Response<string>.CreateFailed(e.Message);
            return BadRequest(response);
        }
    }
    
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var result = await handler.Get(id);
            var response = Response<ClientDto>.CreateSuccessful(result);
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
    public async Task<IActionResult> Create([FromBody] CreateClientDto dto)
    {
        try
        {
            var id = await handler.Create(dto);
            var response = Response<string>.CreateSuccessful(id);
            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            var response = Response<string>.CreateFailed(e.Message);
            return BadRequest(response);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] CreateClientDto dto)
    {
        try
        {
            await handler.Update(id, dto);
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

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await handler.Delete(id);
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