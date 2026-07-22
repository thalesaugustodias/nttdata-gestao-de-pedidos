using MediatR;
using OrderManagement.Application.Interfaces;

namespace OrderManagement.Application.CQRS.Commands.Orders;

public class CancelOrderHandler(IOrderRepository repository) : IRequestHandler<CancelOrderCommand>
{
    public async Task Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await repository.GetByIdAsync(request.Id, cancellationToken)
            ?? throw new KeyNotFoundException($"Order {request.Id} not found.");

        order.Cancel();

        await repository.SaveChangesAsync(cancellationToken);
    }
}
