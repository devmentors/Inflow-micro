namespace Inflow.Services.Payments.Shared.Exceptions;

internal class InvalidCurrencyException : CustomException
{
    public string Currency { get; }

    public InvalidCurrencyException(string currency) : base($"Currency: '{currency}' is invalid.")
    {
        Currency = currency;
    }
}