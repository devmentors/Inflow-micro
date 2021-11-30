using System;
using Inflow.Services.Wallets.Application.Exceptions;

namespace Inflow.Services.Wallets.Application.Wallets.Exceptions;

internal class WalletAlreadyExistsException : AppException
{
    public Guid OwnerId { get; }
    public string Currency { get; }
        
    public WalletAlreadyExistsException(Guid ownerId, string currency)
        : base($"Wallet for owner with ID: '{ownerId}', currency: '{currency}' already exists.")
    {
        OwnerId = ownerId;
        Currency = currency;
    }
}