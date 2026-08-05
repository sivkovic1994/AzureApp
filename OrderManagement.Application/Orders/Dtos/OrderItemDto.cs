using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Orders.Dtos;

public record OrderItemDto(
    Guid ProductId,
    string ProductName,
    decimal UnitPrice,
    int Quantity,
    decimal LineTotal)
{
    public static OrderItemDto FromEntity(OrderItem item) => new(
        item.ProductId,
        item.ProductName,
        item.UnitPrice.Amount,
        item.Quantity,
        item.LineTotal.Amount);
}
