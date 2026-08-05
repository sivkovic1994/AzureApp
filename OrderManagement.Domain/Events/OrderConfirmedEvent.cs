using OrderManagement.Domain.Common;

namespace OrderManagement.Domain.Events;

public sealed class OrderConfirmedEvent : IDomainEvent
{
    public Guid OrderId { get; }
    public Guid CustomerId { get; }
    public DateTime OccurredOn { get; }

    public OrderConfirmedEvent(Guid orderId, Guid customerId)
    {
        OrderId = orderId;
        CustomerId = customerId;
        OccurredOn = DateTime.UtcNow;
    }
}
