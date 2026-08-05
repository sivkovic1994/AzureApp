namespace OrderManagement.Application.Products.Dtos;

public record CreateProductRequest(
    string Name,
    string Sku,
    decimal Price,
    string Currency,
    int InitialStock);
