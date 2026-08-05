namespace OrderManagement.Application.Orders.Dtos;

public record AddOrderItemRequest(Guid ProductId, int Quantity);
