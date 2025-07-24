using ClientDirectory.Domain.Common;

namespace ClientDirectory.Domain.Entities;

public class Movement : BaseEntity
{
    public DateTime Date { get; set; }
    public int Type { get; set; }
    public decimal Value { get; set; }
    public decimal PreviousBalance { get; set; }
    public string AccountId { get; set; }
    public Account Account { get; set; }
}