using System;
using Inflow.Services.Wallets.Application.Exceptions;

namespace Inflow.Services.Wallets.Application.Wallets.Exceptions
{
    internal class WalletNotFoundException : AppException
    {
        public Guid OwnerId { get; }
        public string Currency { get; }
        public Guid WalletId { get; }

        public WalletNotFoundException(Guid walletId) : base($"Wallet with ID: '{walletId}' was not found.")
        {
            WalletId = walletId;
        }
        
        public WalletNotFoundException(Guid ownerId, string currency)
            : base($"Wallet for owner with ID: '{ownerId}', currency: '{currency}' was not found.")
        {
            OwnerId = ownerId;
            Currency = currency;
        }
    }
}