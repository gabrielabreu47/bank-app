using ClientDirectory.Domain.Entities;
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

    public static implicit operator AccountDto(ClientDirectory.Domain.Entities.Account a)
    {
        if (a == null) return null;
        return new AccountDto
        {
            Id = a.Id,
            AccountNumber = a.AccountNumber,
            Type = (AccountTypes?)a.Type,
            Balance = a.Balance,
            Status = a.Status,
            ClientId = a.ClientId
        };
    }
}