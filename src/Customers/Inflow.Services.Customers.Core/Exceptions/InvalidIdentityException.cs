namespace Inflow.Services.Customers.Core.Exceptions;

internal class InvalidIdentityException : CustomException
{
    public string Type { get; }

    public InvalidIdentityException(string type, string series)
        : base($"Identity type: '{type}', series: '{series}' is invalid.")
    {
        Type = type;
    }
}