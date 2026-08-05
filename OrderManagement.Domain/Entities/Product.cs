using OrderManagement.Domain.Common;
using OrderManagement.Domain.Exceptions;

namespace OrderManagement.Domain.Entities;

public class Product : Entity
{
    public string Name { get; private set; } = null!;
    public string Sku { get; private set; } = null!;
    public Money Price { get; private set; } = null!;
    public int StockQuantity { get; private set; }

    private Product()
    {
    }

    public static Product Create(string name, string sku, Money price, int initialStock)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Product name is required.");
        if (string.IsNullOrWhiteSpace(sku))
            throw new DomainException("Product SKU is required.");
        if (initialStock < 0)
            throw new DomainException("Initial stock cannot be negative.");

        return new Product
        {
            Name = name,
            Sku = sku,
            Price = price,
            StockQuantity = initialStock
        };
    }

    public void ReserveStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity to reserve must be positive.");
        if (quantity > StockQuantity)
            throw new DomainException($"Insufficient stock for '{Name}'. Requested {quantity}, available {StockQuantity}.");

        StockQuantity -= quantity;
    }

    public void ReleaseStock(int quantity)
    {
        if (quantity <= 0)
            throw new DomainException("Quantity to release must be positive.");

        StockQuantity += quantity;
    }
}
