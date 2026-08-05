using OrderManagement.Application.Customers.Dtos;

namespace OrderManagement.Application.Customers;

public interface ICustomerService
{
    Task<CustomerDto> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default);
}
