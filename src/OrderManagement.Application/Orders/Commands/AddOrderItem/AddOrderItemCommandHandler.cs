using MediatR;
using OrderManagement.Application.Common.Exceptions;
using OrderManagement.Application.Common.Interfaces;
using OrderManagement.Application.Orders.Dtos;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Orders.Commands.AddOrderItem;

public class AddOrderItemCommandHandler(
    IOrderRepository orderRepository,
    IProductRepository productRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<AddOrderItemCommand, OrderDto>
{
    public async Task<OrderDto> Handle(AddOrderItemCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), request.OrderId);

        var product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), request.ProductId);

        product.ReserveStock(request.Quantity);
        order.AddItem(product.Id, product.Name, product.Price, request.Quantity);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return OrderDto.FromEntity(order);
    }
}
