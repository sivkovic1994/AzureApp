using OrderManagement.Application.Products.Dtos;

namespace OrderManagement.Application.Products;

public interface IProductService
{
    Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default);
    Task<List<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default);
}
