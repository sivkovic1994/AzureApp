using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Common.Interfaces;

public interface IProductRepository
{
    Task<Product?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<List<Product>> GetAllAsync(CancellationToken cancellationToken = default);
    void Add(Product product);
}
