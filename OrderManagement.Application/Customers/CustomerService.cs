using FluentValidation;
using OrderManagement.Application.Common.Interfaces;
using OrderManagement.Application.Customers.Dtos;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Customers;

public class CustomerService(
    ICustomerRepository customerRepository,
    IUnitOfWork unitOfWork,
    IValidator<CreateCustomerRequest> createValidator)
    : ICustomerService
{
    public async Task<CustomerDto> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        await createValidator.ValidateAndThrowAsync(request, cancellationToken);

        var customer = Customer.Create(request.Name, request.Email);

        customerRepository.Add(customer);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return CustomerDto.FromEntity(customer);
    }
}
