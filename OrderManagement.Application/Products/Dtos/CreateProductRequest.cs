namespace OrderManagement.Application.Products.Dtos;

public class CreateProductRequest
{
    public string Name { get; }
    public string Sku { get; }
    public decimal Price { get; }
    public string Currency { get; }
    public int InitialStock { get; }

    public CreateProductRequest(string name, string sku, decimal price, string currency, int initialStock)
    {
        Name = name;
        Sku = sku;
        Price = price;
        Currency = currency;
        InitialStock = initialStock;
    }
}
