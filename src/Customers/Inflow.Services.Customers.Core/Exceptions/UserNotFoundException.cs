namespace Inflow.Services.Customers.Core.Exceptions;

internal class UserNotFoundException : CustomException
{
    public string Email { get; }
        
    public UserNotFoundException(string email) : base($"User with email: '{email}' was not found.")
    {
        Email = email;
    }
}