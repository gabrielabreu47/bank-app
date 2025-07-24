using Application.Dtos.Movement;
using Application.Helpers;
using ClientDirectory.Domain.Common;
using ClientDirectory.Domain.Entities;

namespace Application.Interfaces;

public interface IMovementHandler : IBaseHandler<Movement, MovementDto>
{
    Task<Paged<MovementDto>> Get(string accountId, Filter filter);
    Task CreateMovement(CreateMovementDto movement);
}