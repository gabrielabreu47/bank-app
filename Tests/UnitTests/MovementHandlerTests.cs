using Application.Dtos.Movement;
using Application.Handlers;
using AutoMapper;
using ClientDirectory.Domain.Common;
using ClientDirectory.Domain.Entities;
using ClientDirectory.Domain.Enums;
using ClientDirectory.Domain.Interfaces;
using Moq;
using Xunit;
using System.Linq.Expressions;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Tests.UnitTests;

public class MovementHandlerTests
{
    private readonly Mock<IRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly MovementHandler _handler;

    public MovementHandlerTests()
    {
        _repositoryMock = new Mock<IRepository>();
        _mapperMock = new Mock<IMapper>();
        Mock<ILogger<MovementHandler>> loggerMock = new();
        Mock<IConfiguration> configuration = new();
        _handler = new MovementHandler(_repositoryMock.Object, _mapperMock.Object, loggerMock.Object, configuration.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnPagedMovements()
    {
        // Arrange
        var accountId = "acc123";
        var filter = new Filter { PageNumber = 1, PageSize = 2 };

        var movements = new List<Movement>
        {
            new() { AccountId = accountId, Value = 100 },
            new() { AccountId = accountId, Value = 200 }
        };

        _repositoryMock.Setup(r => r.AsQueryable<Movement>())
            .Returns(movements.AsQueryable());

        _repositoryMock.Setup(r => r.CountAsync(It.IsAny<IQueryable<Movement>>()))
            .ReturnsAsync(movements.Count);

        _repositoryMock.Setup(r => r.ExecuteQuery(It.IsAny<IQueryable<Movement>>()))
            .ReturnsAsync(movements);

        _mapperMock.Setup(m => m.Map<List<MovementDto>>(It.IsAny<List<Movement>>()))
            .Returns(movements.Select(m => new MovementDto { Value = m.Value }).ToList());

        // Act
        var result = await _handler.Get(accountId, filter);

        // Assert
        Assert.Equal(2, result.Items.Count);
        Assert.Equal(100, result.Items[0].Value);
    }

    [Fact]
    public async Task CreateMovement_ShouldAddMovementAndUpdateAccount()
    {
        // Arrange
        var account = new Account { Id = "acc123", Balance = 500 };
        var dto = new CreateMovementDto
        {
            AccountId = account.Id,
            Type = MovementTypes.Credit,
            Value = 200
        };

        var movementEntity = new Movement { AccountId = dto.AccountId, Value = dto.Value };

        _repositoryMock.Setup(r => r.FirstOrDefault(It.IsAny<Expression<Func<Account, bool>>>()))
            .ReturnsAsync(account);

        _mapperMock.Setup(m => m.Map<Movement>(dto)).Returns(movementEntity);

        _repositoryMock.Setup(r => r.UpdateAsync(It.IsAny<Account>())).Returns(Task.CompletedTask);
        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Movement>())).Returns(Task.CompletedTask);

        // Act
        await _handler.CreateMovement(dto);

        // Assert
        _repositoryMock.Verify(r => r.UpdateAsync(It.Is<Account>(a => a.Balance == 700)), Times.Once);
        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Movement>()), Times.Once);
    }
}