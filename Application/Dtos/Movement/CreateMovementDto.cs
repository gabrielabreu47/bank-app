using ClientDirectory.Domain.Enums;

namespace Application.Dtos.Movement;

public class CreateMovementDto
{
    public MovementTypes? Type { get; set; }
    public decimal Value { get; set; }
    public string AccountId { get; set; }
}