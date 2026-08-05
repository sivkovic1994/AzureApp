using MediatR;
using OrderManagement.Application.Customers.Dtos;

namespace OrderManagement.Application.Customers.Commands.CreateCustomer;

public record CreateCustomerCommand(string Name, string Email) : IRequest<CustomerDto>;
