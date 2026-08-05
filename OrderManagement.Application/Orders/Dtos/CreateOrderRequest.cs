namespace OrderManagement.Application.Orders.Dtos;

public record CreateOrderRequest(Guid CustomerId, string Currency = "EUR");
