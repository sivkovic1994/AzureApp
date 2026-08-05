using FluentValidation;
using OrderManagement.Application.Common.Interfaces;
using OrderManagement.Application.Products.Dtos;
using OrderManagement.Domain.Common;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Products;

public class ProductService(
    IProductRepository productRepository,
    IUnitOfWork unitOfWork,
    IValidator<CreateProductRequest> createValidator)
    : IProductService
{
    public async Task<ProductDto> CreateAsync(CreateProductRequest request, CancellationToken cancellationToken = default)
    {
        await createValidator.ValidateAndThrowAsync(request, cancellationToken);

        var product = Product.Create(
            request.Name,
            request.Sku,
            Money.Of(request.Price, request.Currency),
            request.InitialStock);

        productRepository.Add(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ProductDto.FromEntity(product);
    }

    public async Task<List<ProductDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var products = await productRepository.GetAllAsync(cancellationToken);
        return products.Select(ProductDto.FromEntity).ToList();
    }
}
