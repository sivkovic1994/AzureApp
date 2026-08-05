namespace OrderManagement.Application.Customers.Dtos;

public class CreateCustomerRequest
{
    public string Name { get; }
    public string Email { get; }

    public CreateCustomerRequest(string name, string email)
    {
        Name = name;
        Email = email;
    }
}
