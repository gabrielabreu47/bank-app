using ClientDirectory.Domain.Enums;

namespace Application.Dtos.Account;

public class ReportDto
{
    public string? ClientName { get; set; }
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public string? AccountNumber { get; set; }
    public bool Status { get; set; }
    public AccountTypes Type { get; set; }
    public decimal Balance { get; set; }
    public List<ReportMovementDto>? Movements { get; set; }
}

public class ReportMovementDto
{
    public DateTime Date { get; set; }
    public MovementTypes Type { get; set; }
    public decimal Value { get; set; }
    public decimal InitialBalance { get; set; }
    public decimal CurrentBalance { get; set; }

    public static implicit operator ReportMovementDto(ClientDirectory.Domain.Entities.Movement m)
    {
        return new ReportMovementDto
        {
            Date = m.Date,
            Type = (MovementTypes)m.Type,
            Value = m.Value,
            InitialBalance = m.PreviousBalance,
            CurrentBalance = m.Type == (int)MovementTypes.Credit ? m.PreviousBalance + m.Value : m.PreviousBalance - m.Value
        };
    }
}

public class ReportFilter
{
    public DateTime From { get; set; }
    public DateTime To { get; set; }
    public string? AccountId { get; set; }
}