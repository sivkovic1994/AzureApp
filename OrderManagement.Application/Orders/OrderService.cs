using FluentValidation;
using Microsoft.Extensions.Logging;
using OrderManagement.Application.Common.Exceptions;
using OrderManagement.Application.Common.Interfaces;
using OrderManagement.Application.Orders.Dtos;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Events;

namespace OrderManagement.Application.Orders;

public class OrderService : IOrderService
{
    private readonly IOrderRepository _orderRepository;
    private readonly IProductRepository _productRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateOrderRequest> _createOrderValidator;
    private readonly IValidator<AddOrderItemRequest> _addItemValidator;
    private readonly ILogger<OrderService> _logger;

    public OrderService(
        IOrderRepository orderRepository,
        IProductRepository productRepository,
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        IValidator<CreateOrderRequest> createOrderValidator,
        IValidator<AddOrderItemRequest> addItemValidator,
        ILogger<OrderService> logger)
    {
        _orderRepository = orderRepository;
        _productRepository = productRepository;
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _createOrderValidator = createOrderValidator;
        _addItemValidator = addItemValidator;
        _logger = logger;
    }

    public async Task<OrderDto> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default)
    {
        await _createOrderValidator.ValidateAndThrowAsync(request, cancellationToken);

        var customer = await _customerRepository.GetByIdAsync(request.CustomerId, cancellationToken)
            ?? throw new NotFoundException(nameof(Customer), request.CustomerId);

        var order = Order.Create(customer.Id, request.Currency);

        _orderRepository.Add(order);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ToDto(order);
    }

    public async Task<OrderDto> AddItemAsync(Guid orderId, AddOrderItemRequest request, CancellationToken cancellationToken = default)
    {
        await _addItemValidator.ValidateAndThrowAsync(request, cancellationToken);

        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), orderId);

        var product = await _productRepository.GetByIdAsync(request.ProductId, cancellationToken)
            ?? throw new NotFoundException(nameof(Product), request.ProductId);

        product.ReserveStock(request.Quantity);
        order.AddItem(product.Id, product.Name, product.Price, request.Quantity);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ToDto(order);
    }

    public async Task<OrderDto> ConfirmAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), orderId);

        order.Confirm();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        HandleDomainEvents(order);

        return ToDto(order);
    }

    public async Task<OrderDto> CancelAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), orderId);

        foreach (var item in order.Items)
        {
            var product = await _productRepository.GetByIdAsync(item.ProductId, cancellationToken);
            product?.ReleaseStock(item.Quantity);
        }

        order.Cancel();
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return ToDto(order);
    }

    public async Task<OrderDto> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default)
    {
        var order = await _orderRepository.GetByIdAsync(orderId, cancellationToken)
            ?? throw new NotFoundException(nameof(Order), orderId);

        return ToDto(order);
    }

    public async Task<List<OrderDto>> GetAllAsync(CancellationToken cancellationToken = default)
    {
        var orders = await _orderRepository.GetAllAsync(cancellationToken);
        return orders.Select(ToDto).ToList();
    }

    private void HandleDomainEvents(Order order)
    {
        foreach (var domainEvent in order.DomainEvents)
        {
            if (domainEvent is OrderConfirmedEvent confirmed)
            {
                _logger.LogInformation(
                    "Order {OrderId} confirmed for customer {CustomerId} at {OccurredOn}. Invoice generation would be triggered here.",
                    confirmed.OrderId,
                    confirmed.CustomerId,
                    confirmed.OccurredOn);
            }
        }

        order.ClearDomainEvents();
    }

    private static OrderDto ToDto(Order order) => new(
        order.Id,
        order.CustomerId,
        order.Status.ToString(),
        order.CreatedOn,
        order.Currency,
        order.TotalAmount.Amount,
        order.Items.Select(i => new OrderItemDto(
            i.ProductId,
            i.ProductName,
            i.UnitPrice.Amount,
            i.Quantity,
            i.LineTotal.Amount)).ToList());
}
