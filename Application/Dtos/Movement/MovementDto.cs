using ClientDirectory.Domain.Enums;

namespace Application.Dtos.Movement;

public class MovementDto
{
    public string? Id { get; set; }
    public DateTime? Date { get; set; }
    public MovementTypes? Type { get; set; }
    public decimal? Value { get; set; }
    public decimal? Balance { get; set; }
}