using MediatR;

namespace OrderManagement.Application.CQRS.Commands.Orders;

public record CancelOrderCommand(Guid Id) : IRequest;
