using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Orders.Dtos;

public record OrderDto(
    Guid Id,
    Guid CustomerId,
    string Status,
    DateTime CreatedOn,
    string Currency,
    decimal TotalAmount,
    List<OrderItemDto> Items)
{
    public static OrderDto FromEntity(Order order) => new(
        order.Id,
        order.CustomerId,
        order.Status.ToString(),
        order.CreatedOn,
        order.Currency,
        order.TotalAmount.Amount,
        order.Items.Select(OrderItemDto.FromEntity).ToList());
}
