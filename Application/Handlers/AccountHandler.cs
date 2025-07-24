using Application.Dtos.Account;
using Application.Interfaces;
using AutoMapper;
using Application.Extensions;
using Application.Helpers;
using ClientDirectory.Domain.Common;
using ClientDirectory.Domain.Entities;
using ClientDirectory.Domain.Enums;
using ClientDirectory.Domain.Interfaces;

namespace Application.Handlers;

public class AccountHandler(IRepository repository, IMapper mapper) 
    : BaseHandler<Account, AccountDto>(repository, mapper), IAccountHandler
{
    private readonly IRepository _repository = repository;
    private readonly IMapper _mapper = mapper;

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

        query = query.Skip(skip).Take(take);

        var response = await _repository.ExecuteQuery(query);

        var result = _mapper.Map<List<AccountDto>>(response);

        return Paged<AccountDto>.Create(result, count, filter.PageNumber, filter.PageSize);
    }

    public async Task<ReportDto> GetAccountReport(ReportFilter filter)
    {
        var account = await _repository.FirstOrDefault<Account>(a => a.Id == filter.AccountId);

        if (account is null) throw new Exception("Cuenta no existe");
        
        var client = await _repository.FirstOrDefault<Client>(c => c.Id == account.ClientId);

        if (client is null) throw new Exception("Cliente no existe");
        

        var movements = await _repository
            .ExecuteQuery<Movement>(m => 
                m.AccountId == filter.AccountId
                && m.Date >= filter.From
                && m.Date <= filter.To);

        var movementReport = movements
            .Select(m => new ReportMovementDto
            {
                Date = m.Date,
                Type = m.Type.ToEnum<MovementTypes>(),
                Value = m.Value,
                InitialBalance = m.PreviousBalance,
                CurrentBalance = GetBalance(m)
            }).ToList();

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

    private static decimal GetBalance(Movement movement)
    {
        return movement.Type.ToEnum<MovementTypes>() switch
        {
            MovementTypes.Credit => movement.PreviousBalance + movement.Value,
            MovementTypes.Debit => movement.PreviousBalance - movement.Value,
            _ => 0
        };
    }
}