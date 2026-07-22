using OrderManagement.Domain.Enums;

namespace OrderManagement.Application.DTOs;

public record OrderItemDto(
    Guid Id,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice
);

public record OrderDto(
    Guid Id,
    Guid CustomerId,
    OrderStatus Status,
    DateTime CreatedAt,
    decimal TotalAmount,
    IReadOnlyCollection<OrderItemDto> Items
);

public record PagedResult<T>(
    IEnumerable<T> Items,
    int TotalCount,
    int Page,
    int PageSize
);
