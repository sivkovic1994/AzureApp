using FluentValidation;
using Microsoft.Extensions.Logging;
using OrderManagement.Application.Common.Exceptions;
using OrderManagement.Application.Common.Interfaces;
using OrderManagement.Application.Orders.Dtos;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Events;

namespace OrderManagement.Application.Orders;

public class OrderService(
    IOrderRepository orderRepository,
    IProductRepository productRepository,
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork,
    IValidator<CreateOrderRequest> createOrderValidator,
    IValidator<AddOrderItemRequest> addItemValidator,
    ILogger<OrderService> logger)
    : IOrderService
{
    public async Task<OrderDto> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        await createOrderValidator.ValidateAndThrowAsync(request, cancellationToken);

        var customer = await customerRepository.GetByIdAsync(request.CustomerId, cancellationToken)
            ?? throw new NotFoundException(nameof(Customer), request.CustomerId);

        var order = Order.Create(customer.Id, request.Currency);

        orderRepository.Add(order);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return OrderDto.FromEntity(order);
    }

    public async Task<OrderDto> AddItemAsync(Guid orderId, AddOrderItemRequest request, CancellationToken cancellationToken = default)
    {
        await addItemValidator.ValidateAndThrowAsync(request, cancellationToken);

        var order = await orderRepository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), orderId);

        var product = await productRepository.GetByIdAsync(request.ProductId, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), request.ProductId);

        product.ReserveStock(request.Quantity);
        order.AddItem(product.Id, product.Name, product.Price, request.Quantity);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return OrderDto.FromEntity(order);
    }

    public async Task<OrderDto> ConfirmAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await orderRepository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), orderId);

        order.Confirm();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        HandleDomainEvents(order);

        return OrderDto.FromEntity(order);
    }

    public async Task<OrderDto> CancelAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await orderRepository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), orderId);

        foreach (var item in order.Items)
        {
            var product = await productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            product?.ReleaseStock(item.Quantity);
        }

        order.Cancel();
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return OrderDto.FromEntity(order);
    }

    public async Task<OrderDto> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await orderRepository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), orderId);

        return OrderDto.FromEntity(order);
    }

    public async Task<List<OrderDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var orders = await orderRepository.GetAllAsync(cancellationToken);
        return orders.Select(OrderDto.FromEntity).ToList();
    }

    private void HandleDomainEvents(Order order)
    {
        foreach (var domainEvent in order.DomainEvents)
        {
            if (domainEvent is OrderConfirmedEvent confirmed)
            {
                logger.LogInformation(
                    "Order {OrderId} confirmed for customer {CustomerId} at {OccurredOn}. Invoice generation would be triggered here.",
                    confirmed.OrderId,
                    confirmed.CustomerId,
                    confirmed.OccurredOn);
            }
        }

        order.ClearDomainEvents();
    }
}
