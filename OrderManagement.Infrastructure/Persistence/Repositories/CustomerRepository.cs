using Microsoft.EntityFrameworkCore;
using OrderManagement.Application.Common.Interfaces;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Infrastructure.Persistence.Repositories;

public class CustomerRepository : ICustomerRepository
{
    private readonly ApplicationDbContext _context;

    public CustomerRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default) =>
        _context.Customers.FirstOrDefaultAsync(c => c.Id == id, cancellationToken);

    public Task<List<Customer>> GetAllAsync(CancellationToken cancellationToken = default) =>
        _context.Customers.AsNoTracking().ToListAsync(cancellationToken);

    public void Add(Customer customer) => _context.Customers.Add(customer);
}
