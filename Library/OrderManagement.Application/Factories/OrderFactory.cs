using OrderManagement.Application.DTOs;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Factories;

public static class OrderFactory
{
    public static OrderDto ToDto(Order order) =>
        new(
            order.Id,
            order.CustomerId,
            order.Status,
            order.CreatedAt,
            order.TotalAmount,
            order.Items.Select(ToDto).ToList().AsReadOnly()
        );

    private static OrderItemDto ToDto(OrderItem item) =>
        new(
            item.Id,
            item.ProductName,
            item.Quantity,
            item.UnitPrice,
            item.TotalPrice
        );
}
