using ClientDirectory.Domain.Enums;

namespace Application.Dtos.Account;

public class AccountDto
{
    public string? Id { get; set; }
    public string? AccountNumber { get; set; }
    public AccountTypes? Type { get; set; }
    public decimal? Balance { get; set; }
    public bool? Status { get; set; }
    public string ClientId { get; set; }
}