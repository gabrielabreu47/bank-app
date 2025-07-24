using ClientDirectory.Domain.Common;

namespace ClientDirectory.Domain.Entities;

public class Account : BaseEntity
{
    public string AccountNumber { get; set; }
    public int Type { get; set; }
    public decimal Balance { get; set; }
    public bool Status { get; set; }
    public string ClientId { get; set; }
    public Client Client { get; set; }
}