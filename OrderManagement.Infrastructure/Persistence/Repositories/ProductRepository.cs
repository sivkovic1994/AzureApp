using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Common.Interfaces;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Infrastructure.Persistence.Repositories;

public class ProductRepository : IProductRepository
{
    private readonly ApplicationDbContext _context;

    public ProductRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.Products.FirstOrDefaultAsync(p => p.Id == id, cancellationToken);

    public Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default) =>
        _context.Products.AsNoTracking().ToListAsync(cancellationToken);

    public void Add(Product product) => _context.Products.Add(product);
}
