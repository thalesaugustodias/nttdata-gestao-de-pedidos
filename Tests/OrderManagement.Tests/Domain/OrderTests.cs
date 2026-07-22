using FluentAssertions;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Tests.Domain;

public class OrderTests
{
    [Fact]
    public void Create_ValidData_ReturnsPendingOrder()
    {
        var customerId = Guid.NewGuid();
        var order = Order.Create(customerId, [("Product A", 2, 50.00m)]);

        order.CustomerId.Should().Be(customerId);
        order.Status.Should().Be(OrderStatus.Pending);
        order.Items.Should().HaveCount(1);
        order.TotalAmount.Should().Be(100.00m);
    }

    [Fact]
    public void TotalAmount_MultipleItems_CalculatesCorrectly()
    {
        var order = Order.Create(Guid.NewGuid(),
        [
            ("Product A", 2, 10.00m),
            ("Product B", 3, 5.00m)
        ]);

        order.TotalAmount.Should().Be(35.00m);
    }

    [Fact]
    public void Cancel_PendingOrder_SetsCancelledStatus()
    {
        var order = Order.Create(Guid.NewGuid(), [("Product A", 1, 10.00m)]);

        order.Cancel();

        order.Status.Should().Be(OrderStatus.Cancelled);
    }

    [Fact]
    public void Cancel_AlreadyCancelled_ThrowsInvalidOperationException()
    {
        var order = Order.Create(Guid.NewGuid(), [("Product A", 1, 10.00m)]);
        order.Cancel();

        var act = () => order.Cancel();

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Only pending orders can be cancelled.");
    }
}
