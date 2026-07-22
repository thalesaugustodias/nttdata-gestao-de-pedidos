using MediatR;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Factories;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.CQRS.Commands.Orders;

public class CreateOrderHandler(IOrderRepository repository) : IRequestHandler<CreateOrderCommand, OrderDto>
{
    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var items = request.Items.Select(i => (i.ProductName, i.Quantity, i.UnitPrice));
        var order = Order.Create(request.CustomerId, items);

        await repository.AddAsync(order, cancellationToken);
        await repository.SaveChangesAsync(cancellationToken);

        return OrderFactory.ToDto(order);
    }
}
