namespace OrderManagement.Application.Products.Dtos;

public record ProductDto(
    Guid Id,
    string Name,
    string Sku,
    decimal Price,
    string Currency,
    int StockQuantity);
