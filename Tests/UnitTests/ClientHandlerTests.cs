using Application.Dtos.Client;
using Application.Handlers;
using AutoMapper;
using ClientDirectory.Domain.Common;
using ClientDirectory.Domain.Entities;
using ClientDirectory.Domain.Interfaces;
using Moq;
using Xunit;

namespace Tests.UnitTests;

public class ClientHandlerTests
{
    private readonly Mock<IRepository> _repositoryMock;
    private readonly Mock<IMapper> _mapperMock;
    private readonly ClientHandler _handler;

    public ClientHandlerTests()
    {
        _repositoryMock = new Mock<IRepository>();
        _mapperMock = new Mock<IMapper>();
        _handler = new ClientHandler(_repositoryMock.Object, _mapperMock.Object);
    }

    [Fact]
    public async Task Get_ShouldReturnPagedClients()
    {
        // Arrange
        var filter = new Filter { PageNumber = 1, PageSize = 2 };

        var clients = new List<Client>
        {
            new() { Name = "Alice", LastName = "Smith" },
            new() { Name = "Bob", LastName = "Johnson" }
        };

        _repositoryMock.Setup(r => r.AsQueryable<Client>())
            .Returns(clients.AsQueryable());

        _repositoryMock.Setup(r => r.CountAsync(It.IsAny<IQueryable<Client>>()))
            .ReturnsAsync(clients.Count);

        _repositoryMock.Setup(r => r.ExecuteQuery(It.IsAny<IQueryable<Client>>()))
            .ReturnsAsync(clients);

        _mapperMock.Setup(m => m.Map<List<ClientDto>>(It.IsAny<List<Client>>()))
            .Returns(clients.Select(c => new ClientDto { Name = c.Name, LastName = c.LastName }).ToList());

        // Act
        var result = await _handler.Get(filter);

        // Assert
        Assert.Equal(2, result.Items.Count);
        Assert.Equal("Alice", result.Items[0].Name);
    }
}