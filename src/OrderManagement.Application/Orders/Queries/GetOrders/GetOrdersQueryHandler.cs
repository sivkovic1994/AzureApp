using MediatR;
using OrderManagement.Application.Common.Interfaces;
using OrderManagement.Application.Orders.Dtos;

namespace OrderManagement.Application.Orders.Queries.GetOrders;

public class GetOrdersQueryHandler(IOrderRepository orderRepository)
    : IRequestHandler<GetOrdersQuery, List<OrderDto>>
{
    public async Task<List<OrderDto>> Handle(GetOrdersQuery request, CancellationToken cancellationToken)
    {
        var orders = await orderRepository.GetAllAsync(cancellationToken);
        return orders.Select(OrderDto.FromEntity).ToList();
    }
}
