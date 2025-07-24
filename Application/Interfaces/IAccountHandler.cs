using Application.Dtos.Account;
using Application.Helpers;
using ClientDirectory.Domain.Common;
using ClientDirectory.Domain.Entities;

namespace Application.Interfaces;

public interface IAccountHandler : IBaseHandler<Account, AccountDto>
{
    Task<Paged<AccountDto>> GetAccounts(string clientId, Filter filter);
    Task<ReportDto> GetAccountReport(ReportFilter filter);
}