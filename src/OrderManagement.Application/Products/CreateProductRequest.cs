namespace OrderManagement.Application.Products;

public record CreateProductRequest(
    string Name,
    string Sku,
    decimal Price,
    string Currency,
    int InitialStock);
