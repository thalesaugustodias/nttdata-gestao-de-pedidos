using MediatR;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Factories;
using OrderManagement.Application.Interfaces;

namespace OrderManagement.Application.CQRS.Queries.Orders;

public class GetOrderByIdHandler(IOrderRepository repository) : IRequestHandler<GetOrderByIdQuery, OrderDto?>
{
    public async Task<OrderDto?> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await repository.GetByIdAsync(request.Id, cancellationToken);
        return order is null ? null : OrderFactory.ToDto(order);
    }
}
