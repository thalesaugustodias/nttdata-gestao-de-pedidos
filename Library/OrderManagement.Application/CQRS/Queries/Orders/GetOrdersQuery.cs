using MediatR;
using OrderManagement.Application.DTOs;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.CQRS.Queries.Orders;

public record GetOrdersQuery(int Page, int PageSize, OrderStatus Status) : IRequest<PagedResult<OrderDto>>;
