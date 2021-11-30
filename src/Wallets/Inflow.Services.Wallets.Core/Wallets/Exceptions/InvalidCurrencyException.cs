using Inflow.Services.Wallets.Core.Shared.Exceptions;

namespace Inflow.Services.Wallets.Core.Wallets.Exceptions;

internal class InvalidCurrencyException : DomainException
{
    public string Currency { get; }

    public InvalidCurrencyException(string currency) : base($"Currency: '{currency}' is invalid.")
    {
        Currency = currency;
    }
}