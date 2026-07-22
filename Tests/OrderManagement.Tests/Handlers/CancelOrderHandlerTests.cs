using FluentAssertions;
using Moq;
using OrderManagement.Application.CQRS.Commands.Orders;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Tests.Handlers;

public class CancelOrderHandlerTests
{
    private readonly Mock<IOrderRepository> _repositoryMock = new();
    private readonly CancelOrderHandler _handler;

    public CancelOrderHandlerTests()
    {
        _handler = new CancelOrderHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_PendingOrder_CancelsSuccessfully()
    {
        var order = Order.Create(Guid.NewGuid(), [("Product A", 1, 10.00m)]);

        _repositoryMock.Setup(r => r.GetByIdAsync(order.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        _repositoryMock.Setup(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()))
            .Returns(Task.CompletedTask);

        await _handler.Handle(new CancelOrderCommand(order.Id), CancellationToken.None);

        order.Status.Should().Be(OrderStatus.Cancelled);
        _repositoryMock.Verify(r => r.SaveChangesAsync(It.IsAny<CancellationToken>()), Times.Once);
    }

    [Fact]
    public async Task Handle_OrderNotFound_ThrowsKeyNotFoundException()
    {
        var id = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order?)null);

        var act = async () => await _handler.Handle(new CancelOrderCommand(id), CancellationToken.None);

        await act.Should().ThrowAsync<KeyNotFoundException>();
    }

    [Fact]
    public async Task Handle_ConfirmedOrder_ThrowsInvalidOperationException()
    {
        var order = Order.Create(Guid.NewGuid(), [("Product A", 1, 10.00m)]);

        // Use reflection to set status to Confirmed for this test
        var statusProp = typeof(Order).GetProperty("Status")!;
        statusProp.SetValue(order, OrderStatus.Confirmed);

        _repositoryMock.Setup(r => r.GetByIdAsync(order.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        var act = async () => await _handler.Handle(new CancelOrderCommand(order.Id), CancellationToken.None);

        await act.Should().ThrowAsync<InvalidOperationException>()
            .WithMessage("Only pending orders can be cancelled.");
    }
}
