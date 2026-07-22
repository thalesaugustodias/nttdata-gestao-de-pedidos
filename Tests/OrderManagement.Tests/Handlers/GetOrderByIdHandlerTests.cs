using FluentAssertions;
using Moq;
using OrderManagement.Application.CQRS.Queries.Orders;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Tests.Handlers;

public class GetOrderByIdHandlerTests
{
    private readonly Mock<IOrderRepository> _repositoryMock = new();
    private readonly GetOrderByIdHandler _handler;

    public GetOrderByIdHandlerTests()
    {
        _handler = new GetOrderByIdHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ExistingOrder_ReturnsOrderDto()
    {
        var order = Order.Create(Guid.NewGuid(), [("Product A", 2, 15.00m)]);

        _repositoryMock.Setup(r => r.GetByIdAsync(order.Id, It.IsAny<CancellationToken>()))
            .ReturnsAsync(order);

        var result = await _handler.Handle(new GetOrderByIdQuery(order.Id), CancellationToken.None);

        result.Should().NotBeNull();
        result!.Id.Should().Be(order.Id);
        result.TotalAmount.Should().Be(30.00m);
    }

    [Fact]
    public async Task Handle_NonExistingOrder_ReturnsNull()
    {
        var id = Guid.NewGuid();

        _repositoryMock.Setup(r => r.GetByIdAsync(id, It.IsAny<CancellationToken>()))
            .ReturnsAsync((Order?)null);

        var result = await _handler.Handle(new GetOrderByIdQuery(id), CancellationToken.None);

        result.Should().BeNull();
    }
}
