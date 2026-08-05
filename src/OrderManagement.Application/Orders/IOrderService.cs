using OrderManagement.Application.Orders.Dtos;

namespace OrderManagement.Application.Orders;

public interface IOrderService
{
    Task<OrderDto> CreateAsync(CreateOrderRequest request, CancellationToken cancellationToken = default);
    Task<OrderDto> AddItemAsync(Guid orderId, AddOrderItemRequest request, CancellationToken cancellationToken = default);
    Task<OrderDto> ConfirmAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<OrderDto> CancelAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<OrderDto> GetByIdAsync(Guid orderId, CancellationToken cancellationToken = default);
    Task<List<OrderDto>> GetAllAsync(CancellationToken cancellationToken = default);
}
