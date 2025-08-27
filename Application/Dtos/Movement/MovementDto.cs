using ClientDirectory.Domain.Entities;
using ClientDirectory.Domain.Enums;

namespace Application.Dtos.Movement;

public class MovementDto
{
    public string? Id { get; set; }
    public DateTime? Date { get; set; }
    public MovementTypes? Type { get; set; }
    public decimal? Value { get; set; }
    public decimal? Balance { get; set; }

    public static implicit operator MovementDto(ClientDirectory.Domain.Entities.Movement m)
    {
        if (m == null) return null;
        return new MovementDto
        {
            Id = m.Id,
            Date = m.Date,
            Type = (MovementTypes?)m.Type,
            Value = m.Value,
            Balance = m.PreviousBalance
        };
    }
}