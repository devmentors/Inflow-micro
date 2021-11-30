using Inflow.Services.Wallets.Core.Shared.Exceptions;

namespace Inflow.Services.Wallets.Core.Wallets.Exceptions;

internal class InvalidTransferAmountException : DomainException
{
    public decimal Amount { get; }

    public InvalidTransferAmountException(decimal amount) : base($"Transfer has invalid amount: '{amount}'.")
    {
        Amount = amount;
    }
}