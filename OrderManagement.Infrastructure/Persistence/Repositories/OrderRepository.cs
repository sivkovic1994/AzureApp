using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Common.Interfaces;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Infrastructure.Persistence.Repositories;

public class OrderRepository : IOrderRepository
{
    private readonly ApplicationDbContext _context;

    public OrderRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // Order is the aggregate root — its Items are always loaded together with it,
    // so callers never see a half-populated aggregate.
    public Task<Order?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.Orders
            .Include(o => o.Items)
            .FirstOrDefaultAsync(o => o.Id == id, cancellationToken);

    public Task<List<Order>> GetAllAsync(CancellationToken cancellationToken = default) =>
        _context.Orders
            .Include(o => o.Items)
            .AsNoTracking()
            .ToListAsync(cancellationToken);

    public void Add(Order order) => _context.Orders.Add(order);
}
