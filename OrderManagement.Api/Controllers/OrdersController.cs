using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.Orders;
using OrderManagement.Application.Orders.Dtos;

namespace OrderManagement.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class OrdersController : ControllerBase
{
    private readonly IOrderService _orderService;

    public OrdersController(IOrderService orderService)
    {
        _orderService = orderService;
    }

    [HttpPost]
    public async Task<ActionResult<OrderDto>> Create(
        [FromBody] CreateOrderRequest request,
        CancellationToken cancellationToken)
    {
        var order = await _orderService.CreateAsync(request, cancellationToken);
        return Ok(order);
    }

    [HttpPost("{orderId:guid}/items")]
    public async Task<ActionResult<OrderDto>> AddItem(
        Guid orderId,
        [FromBody] AddOrderItemRequest request,
        CancellationToken cancellationToken)
    {
        var order = await _orderService.AddItemAsync(orderId, request, cancellationToken);
        return Ok(order);
    }

    [HttpPost("{orderId:guid}/confirm")]
    public async Task<ActionResult<OrderDto>> Confirm(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await _orderService.ConfirmAsync(orderId, cancellationToken);
        return Ok(order);
    }

    [HttpPost("{orderId:guid}/cancel")]
    public async Task<ActionResult<OrderDto>> Cancel(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await _orderService.CancelAsync(orderId, cancellationToken);
        return Ok(order);
    }

    [HttpGet("{orderId:guid}")]
    public async Task<ActionResult<OrderDto>> GetById(Guid orderId, CancellationToken cancellationToken)
    {
        var order = await _orderService.GetByIdAsync(orderId, cancellationToken);
        return Ok(order);
    }

    [HttpGet]
    public async Task<ActionResult<List<OrderDto>>> GetAll(CancellationToken cancellationToken)
    {
        var orders = await _orderService.GetAllAsync(cancellationToken);
        return Ok(orders);
    }
}
