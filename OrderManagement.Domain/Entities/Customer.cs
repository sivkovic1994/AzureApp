using OrderManagement.Domain.Common;
using OrderManagement.Domain.Exceptions;

namespace OrderManagement.Domain.Entities;

public class Customer : Entity
{
    public string Name { get; private set; } = null!;
    public string Email { get; private set; } = null!;

    private Customer()
    {
    }

    public static Customer Create(string name, string email)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new DomainException("Customer name is required.");
        if (string.IsNullOrWhiteSpace(email) || !email.Contains('@'))
            throw new DomainException("A valid customer email is required.");

        return new Customer
        {
            Name = name,
            Email = email
        };
    }
}
