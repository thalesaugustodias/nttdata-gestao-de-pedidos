using MediatR;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Factories;
using OrderManagement.Application.Interfaces;

namespace OrderManagement.Application.CQRS.Queries.Orders;

public class GetOrdersHandler(IOrderRepository repository) : IRequestHandler<GetOrdersQuery, PagedResult<OrderDto>>
{
    public async Task<PagedResult<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var (items, totalCount) = await repository.GetPagedAsync(request.Page, request.PageSize, request.Status, cancellationToken);

        var dtos = items.Select(OrderFactory.ToDto);

        return new PagedResult<OrderDto>(dtos, totalCount, request.Page, request.PageSize);
    }
}
