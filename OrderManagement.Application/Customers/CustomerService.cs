using FluentValidation;
using OrderManagement.Application.Common.Interfaces;
using OrderManagement.Application.Customers.Dtos;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Customers;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidator<CreateCustomerRequest> _createValidator;

    public CustomerService(
        ICustomerRepository customerRepository,
        IUnitOfWork unitOfWork,
        IValidator<CreateCustomerRequest> createValidator)
    {
        _customerRepository = customerRepository;
        _unitOfWork = unitOfWork;
        _createValidator = createValidator;
    }

    public async Task<CustomerDto> CreateAsync(CreateCustomerRequest request, CancellationToken cancellationToken = default)
    {
        await _createValidator.ValidateAndThrowAsync(request, cancellationToken);

        var customer = Customer.Create(request.Name, request.Email);

        _customerRepository.Add(customer);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return new CustomerDto(customer.Id, customer.Name, customer.Email);
    }
}
