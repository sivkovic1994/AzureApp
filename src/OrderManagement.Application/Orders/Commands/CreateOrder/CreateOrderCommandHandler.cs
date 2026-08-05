using MediatR;
using OrderManagement.Application.Common.Exceptions;
using OrderManagement.Application.Common.Interfaces;
using OrderManagement.Application.Orders.Dtos;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Orders.Commands.CreateOrder;

public class CreateOrderCommandHandler(
    ICustomerRepository customerRepository,
    IOrderRepository orderRepository,
    IUnitOfWork unitOfWork)
    : IRequestHandler<CreateOrderCommand, OrderDto>
{
    public async Task<OrderDto> Handle(CreateOrderCommand request, CancellationToken cancellationToken)
    {
        var customer = await customerRepository.GetByIdAsync(request.CustomerId, cancellationToken)
            ?? throw new NotFoundException(nameof(Customer), request.CustomerId);

        var order = Order.Create(customer.Id, request.Currency);

        orderRepository.Add(order);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return OrderDto.FromEntity(order);
    }
}
