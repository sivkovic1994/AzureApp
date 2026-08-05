using MediatR;
using OrderManagement.Application.Common.Exceptions;
using OrderManagement.Application.Common.Interfaces;
using OrderManagement.Application.Orders.Dtos;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Orders.Queries.GetOrderById;

public class GetOrderByIdQueryHandler(IOrderRepository orderRepository)
    : IRequestHandler<GetOrderByIdQuery, OrderDto>
{
    public async Task<OrderDto> Handle(GetOrderByIdQuery request, CancellationToken cancellationToken)
    {
        var order = await orderRepository.GetByIdAsync(request.OrderId, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), request.OrderId);

        return OrderDto.FromEntity(order);
    }
}
