using MediatR;
using OrderManagement.Application.Products.Dtos;

namespace OrderManagement.Application.Products.Commands.CreateProduct;

public record CreateProductCommand(
    string Name,
    string Sku,
    decimal Price,
    string Currency,
    int InitialStock) : IRequest<ProductDto>;
