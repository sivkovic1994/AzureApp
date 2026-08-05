using MediatR;
using OrderManagement.Application.Orders.Dtos;

namespace OrderManagement.Application.Orders.Commands.ConfirmOrder;

public record ConfirmOrderCommand(Guid OrderId) : IRequest<OrderDto>;
