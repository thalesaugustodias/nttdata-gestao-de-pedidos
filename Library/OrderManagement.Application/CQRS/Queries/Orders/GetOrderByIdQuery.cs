using MediatR;
using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.CQRS.Queries.Orders;

public record GetOrderByIdQuery(Guid Id) : IRequest<OrderDto?>;
