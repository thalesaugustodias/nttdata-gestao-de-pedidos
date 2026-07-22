using MediatR;
using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.CQRS.Commands.Orders;

public record OrderItemInput(string ProductName, int Quantity, decimal UnitPrice);

public record CreateOrderCommand(Guid CustomerId, IEnumerable<OrderItemInput> Items)
    : IRequest<OrderDto>;
