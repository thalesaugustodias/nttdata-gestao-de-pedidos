using FluentAssertions;
using Moq;
using OrderManagement.Application.CQRS.Commands.Orders;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Tests.Handlers;

public class CreateOrderHandlerTests
{
    private readonly Mock<IOrderRepository> _repositoryMock = new();
    private readonly CreateOrderHandler _handler;

    public CreateOrderHandlerTests()
    {
        _handler = new CreateOrderHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ValidCommand_ReturnsOrderDto()
    {
        var command = new CreateOrderCommand(
            CustomerId: Guid.NewGuid(),
            Items: [new OrderItemInput("Product A", 2, 50.00m)]
        );

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _repositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        var result = await _handler.Handle(command, CancellationToken.None);

        result.Should().NotBeNull();
        result.CustomerId.Should().Be(command.CustomerId);
        result.Status.Should().Be(OrderStatus.Pending);
        result.Items.Should().HaveCount(1);
        result.TotalAmount.Should().Be(100.00m);
    }

    [Fact]
    public async Task Handle_ValidCommand_CallsRepositoryAddAndSave()
    {
        var command = new CreateOrderCommand(
            CustomerId: Guid.NewGuid(),
            Items: [new OrderItemInput("Product B", 1, 25.00m)]
        );

        _repositoryMock.Setup(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        _repositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _handler.Handle(command, CancellationToken.None);

        _repositoryMock.Verify(r => r.AddAsync(It.IsAny<Order>(), It.IsAny<CancellationToken>()), Times.Once);
        _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }
}
