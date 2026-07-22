using MediatR;
using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.CQRS.Queries.Orders;

public record GetOrdersQuery(int Page, int PageSize) : IRequest<PagedResult<OrderDto>>;
