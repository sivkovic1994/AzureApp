using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Customers.Dtos;

public record CustomerDto(Guid Id, string Name, string Email)
{
    public static CustomerDto FromEntity(Customer customer) => new(
        customer.Id,
        customer.Name,
        customer.Email);
}
