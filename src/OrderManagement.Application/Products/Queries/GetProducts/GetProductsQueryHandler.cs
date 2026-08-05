using MediatR;
using OrderManagement.Application.Common.Interfaces;
using OrderManagement.Application.Products.Dtos;

namespace OrderManagement.Application.Products.Queries.GetProducts;

public class GetProductsQueryHandler(IProductRepository productRepository)
    : IRequestHandler<GetProductsQuery, List<ProductDto>>
{
    public async Task<List<ProductDto>> Handle(GetProductsQuery request, CancellationToken cancellationToken)
    {
        var products = await productRepository.GetAllAsync(cancellationToken);
        return products.Select(ProductDto.FromEntity).ToList();
    }
}
