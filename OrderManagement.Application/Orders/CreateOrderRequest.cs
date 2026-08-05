namespace OrderManagement.Application.Orders;

public record CreateOrderRequest(Guid CustomerId, string Currency = "EUR");
