using Application.Dtos.Client;
using Application.Helpers;
using Application.Interfaces;
using ClientDirectory.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientController(IClientHandler handler, ILogger<ClientController> logger) : ControllerBase
{
    /// <summary>
    /// Gets all clients with optional filtering.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Filter filter)
    {
        try
        {
            var result = await handler.Get(filter);
            var response = Response<Paged<ClientDto>>.CreateSuccessful(result);
            logger.LogInformation("Clients retrieved with filter");
            return Ok(response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving clients");
            return StatusCode(500, Response<string>.CreateFailed("Internal server error"));
        }
    }
    /// <summary>
    /// Gets a client by its ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var result = await handler.Get(id);
            var response = Response<ClientDto>.CreateSuccessful(result);
            logger.LogInformation("Client retrieved for id: {Id}", id);
            return Ok(response);
        }
        catch (ClientNotFoundException ex)
        {
            logger.LogWarning(ex, "Client not found for id: {Id}", id);
            return NotFound(Response<string>.CreateFailed("Client not found"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving client for id: {Id}", id);
            return StatusCode(500, Response<string>.CreateFailed("Internal server error"));
        }
    }
    /// <summary>
    /// Creates a new client.
    /// Returns 201 Created with the location of the new client.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateClientDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errorMessages = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(Response<string>.CreateFailed($"Validation error: {errorMessages}"));
        }
        try
        {
            var id = await handler.Create(dto);
            var response = Response<string>.CreateSuccessful(id);
            logger.LogInformation("Client created with id: {Id}", id);
            return CreatedAtAction(nameof(GetById), new { id }, response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating client");
            return StatusCode(500, Response<string>.CreateFailed("Internal server error"));
        }
    }
    /// <summary>
    /// Updates an existing client.
    /// Returns 404 if not found.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] CreateClientDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errorMessages = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(Response<string>.CreateFailed($"Validation error: {errorMessages}"));
        }
        try
        {
            await handler.Update(id, dto);
            var response = Response<string>.CreateSuccessful();
            logger.LogInformation("Client updated with id: {Id}", id);
            return Ok(response);
        }
        catch (ClientNotFoundException ex)
        {
            logger.LogWarning(ex, "Client not found for id: {Id}", id);
            return NotFound(Response<string>.CreateFailed("Client not found"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating client for id: {Id}", id);
            return StatusCode(500, Response<string>.CreateFailed("Internal server error"));
        }
    }
    /// <summary>
    /// Deletes a client.
    /// Returns 404 if not found.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await handler.Delete(id);
            var response = Response<string>.CreateSuccessful();
            logger.LogInformation("Client deleted with id: {Id}", id);
            return Ok(response);
        }
        catch (ClientNotFoundException ex)
        {
            logger.LogWarning(ex, "Client not found for id: {Id}", id);
            return NotFound(Response<string>.CreateFailed("Client not found"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting client for id: {Id}", id);
            return StatusCode(500, Response<string>.CreateFailed("Internal server error"));
        }
    }
}