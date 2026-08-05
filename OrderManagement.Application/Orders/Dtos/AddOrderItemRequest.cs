namespace OrderManagement.Application.Orders.Dtos;

public class AddOrderItemRequest
{
    public Guid ProductId { get; }
    public int Quantity { get; }

    public AddOrderItemRequest(Guid productId, int quantity)
    {
        ProductId = productId;
        Quantity = quantity;
    }
}
