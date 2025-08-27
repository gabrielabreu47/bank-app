using System.Linq.Expressions;
using Application.Dtos.Movement;
using Application.Interfaces;
using AutoMapper;
using Application.Helpers;
using Application.Extensions;
using ClientDirectory.Domain.Common;
using ClientDirectory.Domain.Entities;
using ClientDirectory.Domain.Enums;
using ClientDirectory.Domain.Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Configuration;

namespace Application.Handlers;

/// <summary>
/// Handles movement-related operations and queries.
/// </summary>
public class MovementHandler(IRepository repository, IMapper mapper,
    ILogger<MovementHandler> logger, IConfiguration configuration) 
    : BaseHandler<Movement, MovementDto>(repository, mapper, logger), IMovementHandler
{
    private readonly IRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    private readonly decimal _dailyWithdrawalLimit = configuration["Bank:DailyWithdrawalLimit"] is not null 
        ? decimal.Parse(configuration["Bank:DailyWithdrawalLimit"]!) 
        : 1000m;

    /// <summary>
    /// Gets paged movements for an account, with optional filters.
    /// </summary>
    public async Task<Paged<MovementDto>> Get(string accountId, Filter filter)
    {
        var includes = new List<Expression<Func<Movement, object>>> { m => m.Account };
        
        var filtered = await _repository
            .ProjectToWithIncludesAsync<Movement, MovementDto>(includes, m => m.AccountId == accountId);
       
        if (filter.Filters is not null)
        {
            filtered = filtered.AsQueryable().Filter(filter.Filters).ToList();
        }
        
        var ordered = filtered.OrderBy(m => m.Date).ToList();
        
        var count = ordered.Count;
        
        var (skip, take) = Paged<MovementDto>.GetPagination(filter.PageNumber, filter.PageSize);
        
        var paged = ordered.Skip(skip).Take(take).ToList();
        
        return Paged<MovementDto>.Create(paged, count, filter.PageNumber, filter.PageSize);
    }
    /// <summary>
    /// Creates a new movement for an account.
    /// Throws AccountNotFoundException, InsufficientFundsException, or DailyLimitExceededException on error.
    /// </summary>
    public async Task CreateMovement(CreateMovementDto movement)
    {
        var account = await _repository.FirstOrDefault<Account>(a => a.Id == movement.AccountId);
        
        if (account is null)
        {
            logger.LogWarning("Account not found for id: {AccountId}", movement.AccountId);
            throw new AccountNotFoundException("Cuenta no encontrada");
        }
        
        if (movement.Type == MovementTypes.Debit && account.Balance == 0)
        {
            logger.LogWarning("Insufficient funds for account id: {AccountId}", movement.AccountId);
            throw new InsufficientFundsException("Saldo no disponible");
        }

        if (movement.Type == MovementTypes.Debit)
        {
            var start = DateTime.Today.Date;
            var end = start.AddDays(1);

            var todayMovements = await _repository.ExecuteQuery<Movement>(m =>
                m.AccountId == movement.AccountId &&
                m.Date >= start && m.Date < end &&
                m.Value < 0);

            var todayWithdrawals = todayMovements.Sum(m => Math.Abs(m.Value));
            var requested = Math.Abs(movement.Value);

            if (todayWithdrawals + requested > _dailyWithdrawalLimit)
            {
                logger.LogWarning("Daily limit exceeded for account id: {AccountId}", movement.AccountId);
                throw new DailyLimitExceededException("Cupo diario excedido");
            }
        }

        var entity = _mapper.Map<Movement>(movement);
        
        entity.Id = Guid.NewGuid().ToString();
        entity.Date = DateTime.UtcNow;
        entity.PreviousBalance = account.Balance;

        var requestedAbs = Math.Abs(movement.Value);
        entity.Value = movement.Type == MovementTypes.Debit ? -requestedAbs : requestedAbs;

        account.Balance += entity.Value;

        await _repository.UpdateAsync(account);
        await _repository.AddAsync(entity);
    }
}