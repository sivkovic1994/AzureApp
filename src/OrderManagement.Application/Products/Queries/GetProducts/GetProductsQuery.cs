using MediatR;
using OrderManagement.Application.Products.Dtos;

namespace OrderManagement.Application.Products.Queries.GetProducts;

public record GetProductsQuery : IRequest<List<ProductDto>>;
