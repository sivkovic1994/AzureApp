using MediatR;
using OrderManagement.Application.Common.Exceptions;
using OrderManagement.Application.Common.Interfaces;
using OrderManagement.Application.Orders.Dtos;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Orders.Commands.ConfirmOrder;

public class ConfirmOrderCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<ConfirmOrderCommand, OrderDto>
{
    public async Task<OrderDto> Handle(ConfirmOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), request.OrderId);

        order.Confirm();

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return OrderDto.FromEntity(order);
    }
}
