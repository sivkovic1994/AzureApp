using MediatR;
using OrderManagement.Application.Orders.Dtos;

namespace OrderManagement.Application.Orders.Queries.GetOrders;

public record GetOrdersQuery : IRequest<List<OrderDto>>;
