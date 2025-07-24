using Application.Dtos.Account;
using Application.Handlers;
using AutoMapper;
using ClientDirectory.Domain.Common;
using ClientDirectory.Domain.Entities;
using ClientDirectory.Domain.Enums;
using ClientDirectory.Domain.Interfaces;
using Moq;
using Xunit;
using System.Linq.Expressions;

namespace Tests.UnitTests;

public class AccountHandlerTests
{
    private readonly Mock<IRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly AccountHandler _handler;

    public AccountHandlerTests()
    {
        _repositoryMock = new Mock<IRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new AccountHandler(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task GetAccounts_ShouldReturnPagedAccounts()
    {
        // Arrange
        var clientId = "client123";
        var filter = new Filter { PageNumber = 1, PageSize = 2 };

        var accounts = new List<Account>
        {
            new() { Id = "1", ClientId = clientId, AccountNumber = "123", Balance = 100 },
            new() { Id = "2", ClientId = clientId, AccountNumber = "456", Balance = 200 }
        };

        _repositoryMock.Setup(r => r.AsQueryable<Account>())
            .Returns(accounts.AsQueryable());

        _repositoryMock.Setup(r => r.CountAsync(It.IsAny<IQueryable<Account>>()))
            .ReturnsAsync(accounts.Count);

        _repositoryMock.Setup(r => r.ExecuteQuery(It.IsAny<IQueryable<Account>>()))
            .ReturnsAsync(accounts);

        _mapperMock.Setup(m => m.Map<List<AccountDto>>(It.IsAny<List<Account>>()))
            .Returns(accounts.Select(a => new AccountDto { AccountNumber = a.AccountNumber }).ToList());

        // Act
        var result = await _handler.GetAccounts(clientId, filter);

        // Assert
        Assert.Equal(2, result.Items.Count);
        Assert.Equal("123", result.Items[0].AccountNumber);
    }

    [Fact]
    public async Task GetAccountReport_ShouldReturnReportDto()
    {
        // Arrange
        var accountId = "acc1";
        var clientId = "cli1";
        var filter = new ReportFilter
        {
            AccountId = accountId,
            From = DateTime.Today.AddDays(-1),
            To = DateTime.Today
        };

        var account = new Account
        {
            Id = accountId,
            ClientId = clientId,
            Balance = 1000,
            Status = true,
            Type = (int)AccountTypes.Savings,
            AccountNumber = "ACC123"
        };

        var client = new Client
        {
            Id = clientId,
            Name = "John",
            LastName = "Doe"
        };

        var movements = new List<Movement>
        {
            new() { Date = DateTime.Today, Type = (int)MovementTypes.Credit, Value = 100, PreviousBalance = 900, AccountId = accountId }
        };

        _repositoryMock.Setup(r => r.FirstOrDefault<Account>(It.IsAny<Expression<Func<Account, bool>>>()))
            .ReturnsAsync(account);

        _repositoryMock.Setup(r => r.FirstOrDefault<Client>(It.IsAny<Expression<Func<Client, bool>>>()))
            .ReturnsAsync(client);

        _repositoryMock.Setup(r => r.ExecuteQuery<Movement>(It.IsAny<Expression<Func<Movement, bool>>>()))
            .ReturnsAsync(movements);

        // Act
        var result = await _handler.GetAccountReport(filter);

        // Assert
        Assert.Equal("ACC123", result.AccountNumber);
        Assert.Equal("John Doe", result.ClientName);
        Assert.Single(result.Movements);
        Assert.Equal(1000, result.Balance);
    }
}