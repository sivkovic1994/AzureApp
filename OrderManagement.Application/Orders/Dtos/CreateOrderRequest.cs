namespace OrderManagement.Application.Orders.Dtos;

public class CreateOrderRequest
{
    public Guid CustomerId { get; }
    public string Currency { get; }

    public CreateOrderRequest(Guid customerId, string currency = "EUR")
    {
        CustomerId = customerId;
        Currency = currency;
    }
}
