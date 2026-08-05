using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Common.Interfaces;

public interface ICustomerRepository
{
    Task<Customer?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    void Add(Customer customer);
}
