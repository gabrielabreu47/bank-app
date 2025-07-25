using Application.Dtos.Account;
using Application.Helpers;
using Application.Interfaces;
using ClientDirectory.Domain.Common;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AccountController(IAccountHandler handler) : ControllerBase
{
    [HttpGet("id")]
    public async Task<IActionResult> GetById(string id)
    {
        try
        {
            var result = await handler.Get(id);
            var response = Response<AccountDto>.CreateSuccessful(result);
            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            var response = Response<string>.CreateFailed(e.Message);
            return BadRequest(response);
        }
    }
    
    [HttpGet("client/{id}")]
    public async Task<IActionResult> GetAll(string id, [FromQuery] Filter filter)
    {
        try
        {
            var result = await handler.GetAccounts(id, filter);
            var response = Response<Paged<AccountDto>>.CreateSuccessful(result);
            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            var response = Response<Paged<AccountDto>>.CreateFailed(e.Message);
            return BadRequest(response);
        }
    }
    
    [HttpGet("report")]
    public async Task<IActionResult> Report([FromQuery] ReportFilter filter)
    {
        try
        {
            var result = await handler.GetAccountReport(filter);
            var response = Response<ReportDto>.CreateSuccessful(result);
            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            var response = Response<ReportDto>.CreateFailed(e.Message);
            return BadRequest(response);
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] AccountDto dto)
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
    public async Task<IActionResult> Update(string id, [FromBody] AccountDto dto)
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