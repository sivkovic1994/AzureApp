namespace OrderManagement.Application.Orders;

public record AddOrderItemRequest(Guid ProductId, int Quantity);
