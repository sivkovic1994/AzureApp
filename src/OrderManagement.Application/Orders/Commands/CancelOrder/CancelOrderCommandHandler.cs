using MediatR;
using OrderManagement.Application.Common.Exceptions;
using OrderManagement.Application.Common.Interfaces;
using OrderManagement.Application.Orders.Dtos;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Orders.Commands.CancelOrder;

public class CancelOrderCommandHandler(
    IOrderRepository orderRepository,
    IProductRepository productRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CancelOrderCommand, OrderDto>
{
    public async Task<OrderDto> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), request.OrderId);

        foreach (var item in order.Items)
        {
            var product = await productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            product?.ReleaseStock(item.Quantity);
        }

        order.Cancel();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return OrderDto.FromEntity(order);
    }
}
