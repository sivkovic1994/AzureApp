using MediatR;
using OrderManagement.Application.Common.Interfaces;
using OrderManagement.Application.Customers.Dtos;
using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Customers.Commands.CreateCustomer;

public class CreateCustomerCommandHandler(ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
    : IRequestHandler<CreateCustomerCommand, CustomerDto>
{
    public async Task<CustomerDto> Handle(CreateCustomerCommand request, CancellationToken cancellationToken)
    {
        var customer = Customer.Create(request.Name, request.Email);

        customerRepository.Add(customer);
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return CustomerDto.FromEntity(customer);
    }
}
