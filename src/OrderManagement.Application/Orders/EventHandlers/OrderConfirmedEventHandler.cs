using MediatR;
using Microsoft.Extensions.Logging;
using OrderManagement.Application.Common.Events;
using OrderManagement.Domain.Events;

namespace OrderManagement.Application.Orders.EventHandlers;

public class OrderConfirmedEventHandler(ILogger<OrderConfirmedEventHandler> logger)
    : INotificationHandler<DomainEventNotification<OrderConfirmedEvent>>
{
    public Task Handle(DomainEventNotification<OrderConfirmedEvent> notification, CancellationToken cancellationToken)
    {
        var orderEvent = notification.DomainEvent;

        logger.LogInformation(
            "Order {OrderId} confirmed for customer {CustomerId} at {OccurredOn}. Invoice generation would be triggered here.",
            orderEvent.OrderId,
            orderEvent.CustomerId,
            orderEvent.OccurredOn);

        return Task.CompletedTask;
    }
}
