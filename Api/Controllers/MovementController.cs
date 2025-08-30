using Application.Dtos.Movement;
using Application.Helpers;
using Application.Interfaces;
using ClientDirectory.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovementController(IMovementHandler handler, ILogger<MovementController> logger) : ControllerBase
{
    
    /// <summary>
    /// Gets all movements with optional filtering.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Filter filter)
    {
        try
        {
            var result = await handler.Get(filter);
            var response = Response<Paged<MovementDto>>.CreateSuccessful(result);
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
    /// Gets all movements for an account.
    /// </summary>
    /// <response code="200">Returns paged movements</response>
    /// <response code="404">Account not found</response>
    /// <response code="500">Internal server error</response>
    [HttpGet("account/{id}")]
    public async Task<IActionResult> GetAll(string id, [FromQuery] Filter filter)
    {
        try
        {
            var result = await handler.Get(id, filter);
            var response = Response<Paged<MovementDto>>.CreateSuccessful(result);
            logger.LogInformation("Movements retrieved for account id: {Id}", id);
            return Ok(response);
        }
        catch (AccountNotFoundException ex)
        {
            logger.LogWarning(ex, "Account not found for id: {Id}", id);
            return NotFound(Response<string>.CreateFailed("Account not found"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving movements for account id: {Id}", id);
            return StatusCode(500, Response<string>.CreateFailed("Internal server error"));
        }
    }

    /// <summary>
    /// Creates a new movement for an account.
    /// Returns 201 Created.
    /// </summary>
    /// <response code="201">Movement created</response>
    /// <response code="400">Validation error</response>
    /// <response code="404">Account not found</response>
    /// <response code="500">Internal server error</response>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateMovementDto dto)
    {
        if (!ModelState.IsValid)
        {
            var errorMessages = string.Join("; ", ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            return BadRequest(Response<string>.CreateFailed($"Validation error: {errorMessages}"));
        }
        try
        {
            await handler.CreateMovement(dto);
            var response = Response<string>.CreateSuccessful();
            logger.LogInformation("Movement created for account id: {AccountId}", dto.AccountId);
            return StatusCode(201, response);
        }
        catch (AccountNotFoundException ex)
        {
            logger.LogWarning(ex, "Account not found for id: {AccountId}", dto.AccountId);
            return NotFound(Response<string>.CreateFailed("Account not found"));
        }
        catch (InsufficientFundsException ex)
        {
            logger.LogWarning(ex, "Insufficient funds for account id: {AccountId}", dto.AccountId);
            return BadRequest(Response<string>.CreateFailed("Insufficient funds"));
        }
        catch (DailyLimitExceededException ex)
        {
            logger.LogWarning(ex, "Daily limit exceeded for account id: {AccountId}", dto.AccountId);
            return BadRequest(Response<string>.CreateFailed("Daily limit exceeded"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating movement for account id: {AccountId}", dto.AccountId);
            return StatusCode(500, Response<string>.CreateFailed("Internal server error"));
        }
    }
}