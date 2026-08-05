using MediatR;
using OrderManagement.Application.Common.Interfaces;
using OrderManagement.Application.Products.Dtos;
using OrderManagement.Domain.Common;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Products.Commands.CreateProduct;

public class CreateProductCommandHandler(IProductRepository productRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateProductCommand, ProductDto>
{
    public async Task<ProductDto> Handle(CreateProductCommand request, CancellationToken cancellationToken)
    {
        var product = Product.Create(
            request.Name,
            request.Sku,
            Money.Of(request.Price, request.Currency),
            request.InitialStock);

        productRepository.Add(product);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return ProductDto.FromEntity(product);
    }
}
