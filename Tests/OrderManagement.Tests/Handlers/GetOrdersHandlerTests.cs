using FluentAssertions;
using Moq;
using OrderManagement.Application.CQRS.Queries.Orders;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Tests.Handlers;

public class GetOrdersHandlerTests
{
    private readonly Mock<IOrderRepository> _repositoryMock = new();
    private readonly GetOrdersHandler _handler;

    public GetOrdersHandlerTests()
    {
        _handler = new GetOrdersHandler(_repositoryMock.Object);
    }

    [Fact]
    public async Task Handle_ReturnsPagedResult()
    {
        var orders = new List<Order>
        {
            Order.Create(Guid.NewGuid(), [("Product A", 1, 10.00m)]),
            Order.Create(Guid.NewGuid(), [("Product B", 2, 20.00m)])
        };

        _repositoryMock.Setup(r => r.GetPagedAsync(1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync((orders, 2));

        var result = await _handler.Handle(new GetOrdersQuery(1, 10), CancellationToken.None);

        result.Should().NotBeNull();
        result.TotalCount.Should().Be(2);
        result.Items.Should().HaveCount(2);
        result.Page.Should().Be(1);
        result.PageSize.Should().Be(10);
    }

    [Fact]
    public async Task Handle_EmptyRepository_ReturnsEmptyPagedResult()
    {
        _repositoryMock.Setup(r => r.GetPagedAsync(1, 10, It.IsAny<CancellationToken>()))
            .ReturnsAsync((new List<Order>(), 0));

        var result = await _handler.Handle(new GetOrdersQuery(1, 10), CancellationToken.None);

        result.TotalCount.Should().Be(0);
        result.Items.Should().BeEmpty();
    }
}
