using OrderManagement.Domain.Common;
using OrderManagement.Domain.Enums;
using OrderManagement.Domain.Events;
using OrderManagement.Domain.Exceptions;

namespace OrderManagement.Domain.Entities;

public class Order : Entity
{
    private readonly List<OrderItem> _items = [];

    public Guid CustomerId { get; private set; }
    public DateTime CreatedOn { get; private set; }
    public OrderStatus Status { get; private set; }
    public string Currency { get; private set; } = "EUR";

    public IReadOnlyCollection<OrderItem> Items => _items.AsReadOnly();

    public Money TotalAmount => _items
        .Select(i => i.LineTotal)
        .Aggregate(Money.Zero(Currency), (total, line) => total.Add(line));

    private Order()
    {
    }

    public static Order Create(Guid customerId, string currency = "EUR")
    {
        if (customerId == Guid.Empty)
            throw new DomainException("Order must belong to a customer.");

        return new Order
        {
            CustomerId = customerId,
            CreatedOn = DateTime.UtcNow,
            Status = OrderStatus.Pending,
            Currency = currency
        };
    }

    public void AddItem(Guid productId, string productName, Money unitPrice, int quantity)
    {
        EnsureIsPending();

        if (unitPrice.Currency != Currency)
            throw new DomainException(
                $"Cannot add '{productName}' priced in {unitPrice.Currency} to an order in {Currency}.");

        var existing = _items.FirstOrDefault(i => i.ProductId == productId);
        if (existing is not null)
            throw new DomainException($"Product '{productName}' is already in the order. Remove it first to change the quantity.");

        _items.Add(OrderItem.Create(productId, productName, unitPrice, quantity));
    }

    public void RemoveItem(Guid productId)
    {
        EnsureIsPending();

        var item = _items.FirstOrDefault(i => i.ProductId == productId)
            ?? throw new DomainException("Item not found in the order.");

        _items.Remove(item);
    }

    public void Confirm()
    {
        EnsureIsPending();

        if (_items.Count == 0)
            throw new DomainException("Cannot confirm an order with no items.");

        Status = OrderStatus.Confirmed;
        AddDomainEvent(new OrderConfirmedEvent(Id, CustomerId));
    }

    public void Ship()
    {
        if (Status != OrderStatus.Confirmed)
            throw new DomainException("Only confirmed orders can be shipped.");

        Status = OrderStatus.Shipped;
    }

    public void Cancel()
    {
        if (Status == OrderStatus.Shipped)
            throw new DomainException("Shipped orders cannot be cancelled.");

        Status = OrderStatus.Cancelled;
    }

    private void EnsureIsPending()
    {
        if (Status != OrderStatus.Pending)
            throw new DomainException($"Order cannot be modified once it is {Status}.");
    }
}
