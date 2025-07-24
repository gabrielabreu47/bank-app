using Application.Dtos.Movement;
using Application.Interfaces;
using AutoMapper;
using Application.Extensions;
using Application.Helpers;
using ClientDirectory.Domain.Common;
using ClientDirectory.Domain.Entities;
using ClientDirectory.Domain.Enums;
using ClientDirectory.Domain.Interfaces;

namespace Application.Handlers;

public class MovementHandler(IRepository repository, IMapper mapper) 
    : BaseHandler<Movement, MovementDto>(repository, mapper), IMovementHandler
{
    private readonly IRepository _repository = repository;
    private readonly IMapper _mapper = mapper;
    
    public async Task<Paged<MovementDto>> Get(string accountId, Filter filter)
    {
        var query = _repository.AsQueryable<Movement>()
            .Where(m => m.AccountId == accountId);
        
        if (filter.Filters is not null)
        {
            query = query.Filter(filter.Filters);
        }

        var count = await _repository.CountAsync(query);

        var (skip, take) = Paged<MovementDto>.GetPagination(filter.PageNumber, filter.PageSize);

        query = query.Skip(skip).Take(take);

        var response = await _repository.ExecuteQuery(query);

        var result = _mapper.Map<List<MovementDto>>(response);

        return Paged<MovementDto>.Create(result, count, filter.PageNumber, filter.PageSize);
    }

    public async Task CreateMovement(CreateMovementDto movement)
    {
        var account = await _repository.FirstOrDefault<Account>(a => a.Id == movement.AccountId);
        
        if (account is null) throw new Exception("Cuenta no encontrada");
        
        var dailyLimit = 1000m;

        if (movement.Type == MovementTypes.Debit & account.Balance <= 0)
            throw new Exception("Saldo no disponible");

        if (movement.Type == MovementTypes.Debit)
        {
            var today = DateOnly.FromDateTime(DateTime.UtcNow);
            
            var todayMovements = await _repository.ExecuteQuery<Movement>(m =>
                m.AccountId == movement.AccountId &&
                m.Date.Date == today.ToDateTime(TimeOnly.MinValue) &&
                m.Value < 0);

            var todayWithdrawals = todayMovements.Sum(m => Math.Abs(m.Value));
            
            var newTotal = todayWithdrawals + Math.Abs(movement.Value);

            if (newTotal > dailyLimit)
                throw new Exception("Cupo diario excedido");
        }

        var entity = _mapper.Map<Movement>(movement);
        
        entity.Id = Guid.NewGuid().ToString();
        entity.Date = DateTime.UtcNow;
        entity.PreviousBalance = account.Balance;

        account.Balance += movement.Value;

        await _repository.UpdateAsync(account);
        await _repository.AddAsync(entity);
    }
}