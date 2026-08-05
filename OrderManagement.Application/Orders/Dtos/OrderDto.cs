namespace OrderManagement.Application.Orders.Dtos;

public record OrderDto(
    Guid Id,
    Guid CustomerId,
    string Status,
    DateTime CreatedOn,
    string Currency,
    decimal TotalAmount,
    List<OrderItemDto> Items);
