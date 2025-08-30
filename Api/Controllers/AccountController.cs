using Application.Dtos.Account;
using Application.Helpers;
using Application.Interfaces;
using ClientDirectory.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(IAccountHandler handler, ILogger<AccountController> logger) : ControllerBase
{
    /// <summary>
    /// Gets all accounts with optional filtering.
    /// </summary>
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] Filter filter)
    {
        try
        {
            var result = await handler.Get(filter);
            var response = Response<Paged<AccountDto>>.CreateSuccessful(result);
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
    /// Gets an account by its ID.
    /// </summary>
    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var result = await handler.Get(id);
            var response = Response<AccountDto>.CreateSuccessful(result);
            logger.LogInformation("Account retrieved for id: {Id}", id);
            return Ok(response);
        }
        catch (ClientNotFoundException ex)
        {
            logger.LogWarning(ex, "Account not found for id: {Id}", id);
            return NotFound(Response<string>.CreateFailed("Account not found"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving account for id: {Id}", id);
            return StatusCode(500, Response<string>.CreateFailed("Internal server error"));
        }
    }

    /// <summary>
    /// Gets all accounts for a client.
    /// </summary>
    [HttpGet("client/{id}")]
    public async Task<IActionResult> GetClientAccounts(string id, [FromQuery] Filter filter)
    {
        try
        {
            var result = await handler.GetAccounts(id, filter);
            var response = Response<Paged<AccountDto>>.CreateSuccessful(result);
            logger.LogInformation("Accounts retrieved for client id: {Id}", id);
            return Ok(response);
        }
        catch (ClientNotFoundException ex)
        {
            logger.LogWarning(ex, "Client not found for id: {Id}", id);
            return NotFound(Response<string>.CreateFailed("Client not found"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error retrieving accounts for client id: {Id}", id);
            return StatusCode(500, Response<string>.CreateFailed("Internal server error"));
        }
    }

    /// <summary>
    /// Gets a report for an account.
    /// </summary>
    [HttpGet("report")]
    public async Task<IActionResult> Report([FromQuery] ReportFilter filter)
    {
        try
        {
            var result = await handler.GetAccountReport(filter);
            var response = Response<ReportDto>.CreateSuccessful(result);
            logger.LogInformation("Account report generated for account id: {AccountId}", filter.AccountId);
            return Ok(response);
        }
        catch (AccountNotFoundException ex)
        {
            logger.LogWarning(ex, "Account not found for id: {AccountId}", filter.AccountId);
            return NotFound(Response<string>.CreateFailed("Account not found"));
        }
        catch (ClientNotFoundException ex)
        {
            logger.LogWarning(ex, "Client not found for account id: {AccountId}", filter.AccountId);
            return NotFound(Response<string>.CreateFailed("Client not found"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error generating report for account id: {AccountId}", filter.AccountId);
            return StatusCode(500, Response<string>.CreateFailed("Internal server error"));
        }
    }

    /// <summary>
    /// Creates a new account.
    /// Returns 201 Created with the location of the new account.
    /// </summary>
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AccountDto dto)
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
            logger.LogInformation("Account created with id: {Id}", id);
            return CreatedAtAction(nameof(GetById), new { id }, response);
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error creating account");
            return StatusCode(500, Response<string>.CreateFailed("Internal server error"));
        }
    }

    /// <summary>
    /// Updates an existing account.
    /// Returns 404 if not found.
    /// </summary>
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(string id, [FromBody] AccountDto dto)
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
            logger.LogInformation("Account updated with id: {Id}", id);
            return Ok(response);
        }
        catch (ClientNotFoundException ex)
        {
            logger.LogWarning(ex, "Account not found for id: {Id}", id);
            return NotFound(Response<string>.CreateFailed("Account not found"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error updating account for id: {Id}", id);
            return StatusCode(500, Response<string>.CreateFailed("Internal server error"));
        }
    }

    /// <summary>
    /// Deletes an account.
    /// Returns 404 if not found.
    /// </summary>
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(string id)
    {
        try
        {
            await handler.Delete(id);
            var response = Response<string>.CreateSuccessful();
            logger.LogInformation("Account deleted with id: {Id}", id);
            return Ok(response);
        }
        catch (ClientNotFoundException ex)
        {
            logger.LogWarning(ex, "Account not found for id: {Id}", id);
            return NotFound(Response<string>.CreateFailed("Account not found"));
        }
        catch (Exception ex)
        {
            logger.LogError(ex, "Error deleting account for id: {Id}", id);
            return StatusCode(500, Response<string>.CreateFailed("Internal server error"));
        }
    }
}