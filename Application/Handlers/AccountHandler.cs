using ClientDirectory.Domain.Entities;
using Application.Dtos.Account;
using Application.Interfaces;
using AutoMapper;
using Application.Extensions;
using Application.Helpers;
using ClientDirectory.Domain.Common;
using ClientDirectory.Domain.Enums;
using ClientDirectory.Domain.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Handlers;

/// <summary>
/// Handles account-related operations and queries.
/// </summary>
public class AccountHandler(IRepository repository, IMapper mapper, ILogger<AccountHandler> logger) 
    : BaseHandler<Account, AccountDto>(repository, mapper, logger), IAccountHandler
{
    private readonly IRepository _repository = repository;

    /// <summary>
    /// Gets an account by its ID.
    /// Throws ClientNotFoundException if not found.
    /// </summary>
    public async Task<AccountDto> Get(string id)
    {
        var result = await _repository.FirstOrDefault<Account>(c => c.Id == id);
        
        if (result is null)
        {
            logger.LogWarning("Client not found for id: {Id}", id);
            throw new ClientNotFoundException("Client doesn't exists");
        }
        
        logger.LogInformation("Account retrieved for id: {Id}", id);
        
        return new AccountDto {
            Id = result.Id,
            AccountNumber = result.AccountNumber,
            Type = result.Type.ToEnum<AccountTypes>(),
            Balance = result.Balance,
            Status = result.Status,
            ClientId = result.ClientId
        };
    }
    
    /// <summary>
    /// Gets paged accounts for a client, with optional filters.
    /// </summary>
    public async Task<Paged<AccountDto>> GetAccounts(string clientId, Filter filter)
    {
        var query = _repository.AsQueryable<Account>()
            .Where(a => a.ClientId == clientId);
        
        if (filter.Filters is not null)
        {
            query = query.Filter(filter.Filters);
        }
        
        var count = await _repository.CountAsync(query);
        
        var (skip, take) = Paged<AccountDto>.GetPagination(filter.PageNumber, filter.PageSize);
        
        var pagedQuery = query
            .Skip(skip)
            .Take(take);
        
        var accounts = await _repository.ExecuteQuery(pagedQuery);

        var result = accounts
            .Select(a => new AccountDto
            {
                Id = a.Id,
                AccountNumber = a.AccountNumber,
                Type = (AccountTypes?)a.Type,
                Balance = a.Balance,
                Status = a.Status,
                ClientId = a.ClientId
            }).ToList();
        
        return Paged<AccountDto>
            .Create(result, count, filter.PageNumber, filter.PageSize);
    }

    /// <summary>
    /// Gets a report for an account, including movements and client info.
    /// Throws AccountNotFoundException or ClientNotFoundException if not found.
    /// </summary>
    public async Task<ReportDto> GetAccountReport(ReportFilter filter)
    {
        var account = await _repository.FirstOrDefault<Account>(a => a.Id == filter.AccountId);
        
        if (account is null)
        {
            logger.LogWarning("Account not found for id: {AccountId}", filter.AccountId);
            throw new AccountNotFoundException("Cuenta no existe");
        }
        
        var client = await _repository.FirstOrDefault<Client>(c => c.Id == account.ClientId);
        
        if (client is null)
        {
            logger.LogWarning("Client not found for account id: {AccountId}", filter.AccountId);
            throw new ClientNotFoundException("Cliente no existe");
        }
        
        filter.To = filter.To.Date.AddDays(1);

        var movementsQuery = _repository.AsQueryable<Movement>()
            .Where(m =>
                m.AccountId == filter.AccountId &&
                m.Date >= filter.From &&
                m.Date <= filter.To);
        
        var movements = await _repository.ExecuteQuery(movementsQuery);
        
        var movementReport = movements
            .OrderBy(m => m.Date)
            .Select(m => new ReportMovementDto {
                Date = m.Date,
                Type = m.Type.ToEnum<MovementTypes>(),
                Value = m.Value,
                CurrentBalance = m.PreviousBalance + (m.Type == (int)MovementTypes.Credit ? m.Value : -m.Value),
                InitialBalance = m.PreviousBalance
            })
            .ToList();
        
        var result = new ReportDto
        {
            From = filter.From,
            To = filter.To,
            Balance = account.Balance,
            Status = account.Status,
            Type = account.Type.ToEnum<AccountTypes>(),
            AccountNumber = account.AccountNumber,
            ClientName = $"{client.Name} {client.LastName}",
            Movements = movementReport
        };

        return result;
    }
}