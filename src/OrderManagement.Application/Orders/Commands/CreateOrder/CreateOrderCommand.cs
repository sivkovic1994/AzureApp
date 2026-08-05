using MediatR;
using OrderManagement.Application.Orders.Dtos;

namespace OrderManagement.Application.Orders.Commands.CreateOrder;

public record CreateOrderCommand(Guid CustomerId, string Currency = "EUR") : IRequest<OrderDto>;
