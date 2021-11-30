using System;
using Inflow.Services.Wallets.Core.Shared.Exceptions;

namespace Inflow.Services.Wallets.Core.Wallets.Exceptions;

internal class InsufficientWalletFundsException : DomainException
{
    public Guid WalletId { get; }

    public InsufficientWalletFundsException(Guid walletId)
        : base($"Insufficient funds for wallet with ID: '{walletId}'.")
    {
        WalletId = walletId;
    }
}