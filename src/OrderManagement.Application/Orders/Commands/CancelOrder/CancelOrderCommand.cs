using MediatR;
using OrderManagement.Application.Orders.Dtos;

namespace OrderManagement.Application.Orders.Commands.CancelOrder;

public record CancelOrderCommand(Guid OrderId) : IRequest<OrderDto>;
