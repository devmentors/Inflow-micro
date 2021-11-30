using Inflow.Services.Wallets.Core.Shared.Exceptions;

namespace Inflow.Services.Wallets.Core.Wallets.Exceptions;

internal class InvalidAmountException : DomainException
{
    public decimal Amount { get; }

    public InvalidAmountException(decimal amount) : base($"Amount: '{amount}' is invalid.")
    {
        Amount = amount;
    }
}