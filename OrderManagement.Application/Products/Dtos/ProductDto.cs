using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Products.Dtos;

public record ProductDto(
    Guid Id,
    string Name,
    string Sku,
    decimal Price,
    string Currency,
    int StockQuantity)
{
    public static ProductDto FromEntity(Product product) => new(
        product.Id,
        product.Name,
        product.Sku,
        product.Price.Amount,
        product.Price.Currency,
        product.StockQuantity);
}
