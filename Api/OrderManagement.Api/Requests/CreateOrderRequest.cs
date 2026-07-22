using OrderManagement.Application.CQRS.Commands.Orders;

namespace OrderManagement.Api.Requests;

public record CreateOrderRequest(IEnumerable<OrderItemInput> Items);
